//ITEM_HOCKEYSTICK
datablock AudioProfile(SlapShot)
{
	filename = "./sounds/Hockey_SlapShot.wav";
	description = AudioClosest3d;
	preload = false;
};

datablock ItemData(HockeyStickItem)
{
	 // Basic Item Properties
	shapeFile = "Add-Ons/Item_Hockey/sticknohands.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui properties
	uiName = "Hockey Stick";
	iconName = "./hockeystick";
	doColorShift = false;
	colorShiftColor = "1.0 0.0 0.0 1.0";

	 // Dynamic properties defined by the scripts
	image = HockeyStickImage;
	canDrop = true;
};

datablock ShapeBaseImageData(HockeyStickImage)
{
   // Basic Item properties
   shapeFile = "Add-Ons/Item_Hockey/stick.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = false;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   HockeyStick = true;

   // Projectile && Ammo.
   item = HockeyStickItem;
   ammo = " ";
   //melee particles shoot from eye node for consistancy
   melee = true;
   doRetraction = false;
   //raise your arm up or not
   armReady = true;

   showBricks = false;

   //casing = " ";

   doColorShift = true;
   colorShiftColor = HockeyStickItem.colorShiftColor;

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.0;
	stateTransitionOnTimeout[0]      = "Ready";

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]                    = "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = true;
	stateTimeoutValue[2]            = 0.01;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateTimeoutValue[3]            = 0.15;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = true;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]		     = true;
	stateSequence[3]                = "Fire";

	stateName[4]                    = "CheckFire";
	stateTransitionOnTriggerUp[4]   = "StopFire";

	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.01;
	stateAllowImageChange[5]        = true;
	stateWaitForTimeout[5]          = true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                  = "onStopFire";
};

datablock ShapeBaseImageData(HockeyStickWPuckImage)
{
   // Basic Item properties
   shapeFile = "Add-Ons/Item_Hockey/stickpuck.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = false;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   HockeyStickWPuck = true;
   // Projectile && Ammo.
   item = "";
   ammo = " ";
   projectile = '';
   projectileType = '';

   //melee particles shoot from eye node for consistancy
   melee = true;
   doRetraction = false;
   //raise your arm up or not
   armReady = true;

   showBricks = false;

   //casing = " ";

   doColorShift = true;
   colorShiftColor = HockeyStickItem.colorShiftColor;

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.0;
	stateTransitionOnTimeout[0]      = "Ready";

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]                    = "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = true;
	stateTimeoutValue[2]            = 0.01;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateTimeoutValue[3]            = 0.15;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = true;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]		     = true;
	stateSequence[3]                = "Fire";

	stateName[4]                    = "CheckFire";
	stateTransitionOnTriggerUp[4]   = "StopFire";

	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.1;
	stateAllowImageChange[5]        = true;
	stateWaitForTimeout[5]          = true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                  = "onStopFire";
};
function HockeyStickImage::onFire(%this,%obj,%slot)
{
	%lengthOfRay = 5;
		
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), %lengthOfRay));

	%hit = containerRayCast(%start, %end, $TypeMasks::PlayerObjectType, %obj);

	%obj.playThread(3, shiftUp);
	if($Sim::Time - %obj.lsteal > 2)
	{
		if(isObject(%hit))
			%image = %hit.getMountedImage(0);
		if(isObject(%image) && %image.HockeyStickWPuck)
		{
			%hit.playThread(3, shiftUp);
			%obj.playThread(3, shiftUp);
			serverPlay3D(Impact1BSound,%hit.getPosition);
			%hit.hasPuck = false;
			%hit.hasSportBall = false;
			%hit.unmountimage( 0 );
			%hit.mountimage(HockeyStickImage, 0);
			%p = new Projectile()
			{
				dataBlock = PuckProjectile;
		    	initialVelocity = 0;
		    	initialPosition = %hit.getMuzzlePoint(%slot);
		   	 	sourceObject = %hit;
		   		sourceSlot = %slot;
		    	client = %hit.client;
			};
			%obj.lsteal = $Sim::Time;
		}
	}
}
function HockeyStickWPuckImage::onFire(%his,%obj,%slot)
{
	%obj.hasPuck = false;
	%obj.hasSportBall = false;
	//echo("no puck :(");
	serverPlay3D(SlapShot,%obj.getPosition());
	%obj.unmountimage( 0 );
	%obj.mountimage(HockeyStickImage, 0);
	%aim = %obj.getMuzzleVector(0);
	if(getword(%aim, 2) < 0)
		%aim = getWord(%aim, 0) SPC getWord(%aim, 1) SPC "0";
	%posScale = vectorAdd(vectorScale(%aim,0.1 * vectorLen(%obj.getVelocity())),vectorScale(%aim,3));
	%position = vectorAdd(%posScale,%obj.getPosition());
	%velocity = vectorScale(%aim,60);
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
}
//Loop for the puck
function GreenHockeyLoop(%obj)
{
	//echo("loop");
	if(%obj.hasPuck)
	{
		//echo("green");
		commandToClient(%obj.client,'BottomPrint',"<color:00FF00>__________________________________________________________________",true,true);
		schedule(500,0,GreenHockeyLoop,%obj);
	}
}
function RedHockeyLoop(%obj)
{
	//echo("loop");
	if(isObject(%obj.getMountedImage(0)))
	{
		if(%obj.getMountedImage(0).getname() $= "HockeyStickImage")
		{
			//echo("red");
			commandToClient(%obj.client,'BottomPrint',"__________________________________________________________________",true,true);
			schedule(500,0,RedHockeyLoop,%obj);
		}
	}
}
//Say he has balls eheh
function HockeyStickWPuckImage::onMount(%this,%obj)
{	
	%obj.hideNode(RHand);
	%obj.hideNode(LHand);
	%obj.hasSportBall = true;
	%obj.hasPuck = true;
	GreenHockeyLoop(%obj);
	parent::onMount(%this,%obj);
}

function HockeyStickImage::onMount(%this,%obj)
{
	%obj.hideNode(RHand);
	%obj.hideNode(LHand);
	//echo("mount");
	if(%obj.hasPuck)
	{
		//echo("u has puck");
		%obj.unmountimage( 0 );
		%obj.mountimage(HockeyStickWPuckImage,0);
	}
	else
	{
		RedHockeyLoop(%obj);
	}
}

function HockeyStickImage::onUnMount(%this,%obj)
{
	%obj.unHideNode(RHand);
	%obj.unHideNode(LHand);
}

function HockeyStickWPuckImage::onUnMount(%this,%obj)
{
	%obj.unHideNode(RHand);
	%obj.unHideNode(LHand);
}