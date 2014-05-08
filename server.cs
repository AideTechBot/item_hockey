//Things to exec
exec("./Item_Skates.cs");
exec("./Item_HockeyStick.cs");
exec("./Item_Puck.cs");
exec("./Item_GoalieStick.cs");
exec("./Item_GoalieSkates.cs");
exec("./Item_Helmet.cs");

//Global vars
$Check::Amount = 5;

//Support functions
function makePositive(%num)
{
	if(%num < 0)
	{
		return %num * -1;
	}
	else
	{
		return %num;
	}
}

function vectorMultiply(%vector1, %vector2)
{
	%component1 = getWord(%vector1, 0);
	%component2 = getWord(%vector1, 1);
	%component3 = getWord(%vector1, 2);

	%component4 = getWord(%vector2, 0);
	%component5 = getWord(%vector2, 1);
	%component6 = getWord(%vector2, 2);

	%fcomponent1 = %component1 * %component4;
	%fcomponent2 = %component2 * %component5;
	%fcomponent3 = %component3 * %component6;

	%fvec = %fcomponent1 SPC %fcomponent2 SPC %fcomponent3;

	return %fvec;
}

function VectorExtend(%vec, %x)
{
    return VectorAdd(%vec, VectorScale(VectorNormalize(%vec), %x));
}

//actual code
function servercmdclearpucks(%client)
{
	
}
package IceHockey
{

	function Armor::onTrigger(%data,%obj,%slot,%val)
	{
		Parent::onTrigger(%data,%obj,%slot,%val);
		//echo("onTrigger invoked");
		if(%obj.getDatablock() == PlayerIceHockeyArmor.getID() || %obj.getDatablock() == PlayerGoalieArmor.getID())
		{
			//echo("yes he has a hockey player");
			if(%slot == 2)
			{
				//echo("stopping him");
				if(($Sim::Time - %obj.lastbrake) > 1)
				{
					serverPlay3D(Shave_Ice,%obj.gettransform());
					%obj.setVelocity("0 0 0");
					%obj.lastbrake = $Sim::Time;
				}
			}
		}
		%image = %obj.getMountedImage(0);
		//Echo(%image);
		if(%slot == 4)
		{

			//HOCKEY PASS
			if(isObject(%image) && %image.HockeyStickWPuck)
			{
				%obj.hasPuck = false;
				%obj.hasSportBall = false;
				//echo("no puck :(");
				serverPlay3D(SlapShot,%obj.getPosition());
				%obj.unmountimage( 0 );
				%obj.mountimage(HockeyStickImage, 0);
				%aim = %obj.getMuzzleVector(0);				
				%aim = getWord(%aim, 0) SPC getWord(%aim, 1) SPC "0";
				%posScale = vectorAdd(vectorScale(%aim,0.1 * vectorLen(%obj.getVelocity())),vectorScale(%aim,5));
				%position = vectorAdd(%posScale,%obj.getPosition());
				%velocity = vectorScale(%aim,40);
				//%velocity = vectorAdd(%velScale,vectorLen(%obj.getVelocity()));
				%p = new item()
				{
					dataBlock = PuckPickupItem;
					lifetime = 40000;
					position = %position;
					sourceObject = %obj;
					sourceSlot = 0;
					client = %obj.client;
				};
				%p.setVelocity(%velocity);
				%p.schedulePop();
				%obj.playThread(3, shiftRight);
			}
			if(isObject(%image) && %image.HockeyStick)
			{
				if(%val)
					%obj.playThread(3,shiftRight);
			}
			//GOALIE PASS
			if(isObject(%image) && %image.GoalieStickWPuck)
			{
				%obj.hasPuck = false;
				%obj.hasSportBall = false;
				//echo("no puck :(");
				serverPlay3D(SlapShot,%obj.getPosition());
				%obj.unmountimage( 0 );
				%obj.mountimage(GoalieStickImage, 0);
				%aim = %obj.getMuzzleVector(0);
				%aim = getWord(%aim, 0) SPC getWord(%aim, 1) SPC "0";
				%posScale = vectorAdd(vectorScale(%aim,0.1 * vectorLen(%obj.getVelocity())),vectorScale(%aim,5));
				%position = vectorAdd(%posScale,%obj.getPosition());
				%velocity = vectorScale(%aim,37);
				//%velocity = vectorAdd(%velScale,vectorLen(%obj.getVelocity()));
				%p = new item()
				{
					dataBlock = PuckPickupItem;
					lifetime = 40000;
					position = %position;
					sourceObject = %obj;
					sourceSlot = 0;
					client = %obj.client;
				};
				%p.setVelocity(%velocity);
				%obj.playThread(3, shiftRight);
			}
			if(isObject(%image) && %image.GoalieStick)
			{
				if(%val)
					%obj.playThread(3,shiftRight);
			}
		}
	}

	function player::skateLoop(%this)
	{
		if(%this.getDatablock() == PlayerIceHockeyArmor.getID() || %this.getDatablock() == PlayerGoalieArmor.getID())
		{
			//echo("skateLOOP WOOOOO!@#@#$@#4");
			%pos = getWords(%this.getPosition(), 0, 1);
			if(%this.s_lastpos $= getWords(%this.getPosition(), 0, 1))
			{
				//echo("same position");
				//do nothing!
			}
			else if(!%this.isMounted())
			{
				//echo("diff position");
				if(makePositive(getWord(%this.getVelocity(),0)) < 3 && makePositive(getWord(%this.getVelocity(),1)) < 3)
				{
					%this.playSkateStep(1);
				}
				else
				{
					%this.playSkateStep(2);
				}

				%this.s_lastpos = getWords(%this.getPosition(), 0, 1);
			}
			//echo("@#@#$@#%@#$@#$NUMBER 2");
			%this.schedule(300, skateloop);
		}
	}


	function player::addItem( %this, %tool )
	{
		if(!isObject(%tool))
		{
			return;
		}

		%tool = %tool.getID();
		%slots = %this.getDataBlock().maxTools;

		for(%i = 0; %i < %slots ; %i++)
		{
			if(!isObject(%this.tool[%i]))
			{
				%this.tool[%i] = %tool;

				if(isObject(%cl = %this.client))
				{
					messageClient(%cl, 'MsgItemPickup', '', %i, %tool);
				}
				break;
			}
		}
	}

	function Armor::onCollision(%this, %obj, %col, %thing, %other) 
	{
		//echo("collided");
		if(%col.hasskates)
		{
				echo("pushing...");
				%vel = vectorAdd(%col.getVelocity(),vectorScale(%player.getVelocity,6));
				echo(%vel);
				%col.setVelocity(%vel);
		}
		parent::OnCollision(%this, %obj, %col, %thing, %other);
	}
	function player::playSkateStep(%this,%type)
	{
		%random = getRandom(1, 2);
		if(%type == 1)
		{
			%sound = "skate_quiet" @ (%random);
		}
		else
		{
			%sound = "skate_loud" @ (%random);
		}
 
		serverplay3d(%sound,%this.getHackPosition() SPC "0 0 1 0");
	}
};
activatePackage(IceHockey);

