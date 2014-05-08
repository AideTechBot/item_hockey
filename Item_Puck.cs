//ITEM_PUCK

datablock ProjectileData(PuckProjectile)
{
	sportBallImage = "PuckImage";
	projectileShapeName = "./puck.dts";
	explosion           = "";
	bounceExplosion     = "";
	particleEmitter     = ballTrailEmitter;
	explodeOnDeath = true;

	brickExplosionRadius = 0;
	brickExplosionImpact = 0;             //destroy a brick if we hit it directly?
	brickExplosionForce  = 0;             
	brickExplosionMaxVolume = 0;          //max volume of bricks that we can destroy
	brickExplosionMaxVolumeFloating = 0;  //max volume of bricks that we can destroy if they aren't connected to the ground (should always be >= brickExplosionMaxVolume)

	sound = "";

	muzzleVelocity      = 0;
    restVelocity        = 2;
	velInheritFactor    = 1.0;

	armingDelay         = 12000;
	lifetime            = 30000;
	fadeDelay           = 11500;
	bounceElasticity    = 0.1;
	bounceFriction      = 0;
	isBallistic         = true;
	gravityMod          = 1;

	hasLight    = false;

	uiName = "Hockey Puck"; 
};

datablock ItemData(PuckItem)
{
	 // Basic Item Properties
	shapeFile = "Add-Ons/Item_Hockey/puck.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.5;
	emap = true;

	//gui properties
	uiName = "Hockey Puck";
	iconName = "./hockeypuck"; //_________________________________________TODO
	doColorShift = false;
	colorShiftColor = "1.0 0.0 0.0 1.0";

	 // Dynamic properties defined by the scripts
	image = PuckImage;
	canDrop = true;
};

datablock ShapeBaseImageData(PuckImage)
{
   // Basic Item properties
   shapeFile = "Add-Ons/Item_Hockey/puck.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "-0.1 0.1 0";
   rotation = eulerToMatrix( "90 90 0" );

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = false;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = PuckItem;
   ammo = " ";
   projectile = "";
   projectileType = "";

   //melee particles shoot from eye node for consistancy
   melee = true;
   doRetraction = false;
   //raise your arm up or not
   armReady = true;

   showBricks = false;

   //casing = " ";

   doColorShift = true;
   colorShiftColor = PuckItem.colorShiftColor;

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


function PuckImage::onFire(%this,%obj,%slot)
{
	//echo(%obj.getMuzzlePoint(%slot));
	//drops the puck
	%objectVelocity = %obj.getVelocity();
	%vector = %obj.getMuzzleVector(%slot);
 	%vector1 = VectorScale(%vector,4);
 	%vector2 = VectorScale(%objectVelocity,0.1);
 	%vel =  VectorAdd(%vector1,%vector2);
	%p = new Projectile()
	{
		dataBlock = PuckProjectile;
    	initialVelocity = %vel;
    	initialPosition = %obj.getMuzzlePoint(%slot);
   	 	sourceObject = %obj;
   		sourceSlot = %slot;
    	client = %obj.client;
	};
	//takes the puck out of the invetory
	messageClient( %obj.client, 'MsgItemPickup', '', %obj.currTool, 0 );
	%obj.tool[ %obj.currTool ] = 0;
	serverCmdUnUseTool( %obj.client );
}

function PuckProjectile::onCollision(%this,%obj,%col,%thing,%other)
{
	echo("PUCK COLLUSIONE!!!1!");
	if( %col.getClassName() $= "fxDTSBrick" )
	{
		echo("onballhit");
		%col.onBallHit( %obj.sourceObject, %obj );

	}
	if(%col.getClassName() $= "Player")
		%image = %col.getMountedImage(0);
	//Echo(%image);
	if(isObject(%image) && %image.HockeyStick )
	{
		echo("reg");
		%col.unmountimage( 0 );
		//echo("HOCKY STIK W PUCK");
		%col.mountImage(HockeyStickWPuckImage, 0);
		//echo("has puck");
		%col.hasPuck = 1;
		%col.hasSportBall = 1;
		%obj.delete();
		//echo("parent dat sheet");
		return;
	}
	else if(isObject(%image) && %image.GoalieStick )
	{
		echo("goalie");
		%col.unmountimage( 0 );
		//echo("HOCKY STIK W PUCK");
		%col.mountImage(GoalieStickWPuckImage, 0);
		//echo("has puck");
		%col.hasPuck = 1;
		%col.hasSportBall = 1;
		%obj.delete();
		return;
	}
	parent::onCollision(%this,%obj,%col,%thing,%other);	
}

function PuckProjectile::onRest(%this,%obj,%col,%fade,%pos,%normal)
{
	//don't spawn more than one item
	//we need this check because onRest can be called immediately after onCollision in the same tick
	if(%obj.haveSpawnedItem)
		return;
   else
      %obj.haveSpawnedItem = 1;

	%item = new item()
	{
		dataBlock = PuckPickupItem;
		scale = %obj.getScale();
		minigame = getMiniGameFromObject( %obj );//%obj.minigame;
		spawnBrick = %obj.spawnBrick;
	};
	missionCleanup.add(%item);
   
	// check if a bot spawned the thing
	// if( isObject( %obj.sourceObject.spawnBrick ) )
		// %item.minigame = %obj.sourceObject.spawnBrick.getGroup().client.minigame;
   
	%rot = hGetAnglesFromVector( vectorNormalize(%obj.lVelocity) );

	// let's get the x y normals
	// %xNorm = mFloor( getWord( %normal, 0 ) );
	// %yNorm = mFloor( getWord( %normal, 1 ) );

	// echo( %normal SPC ":" SPC %xNorm SPC %yNorm );
	// let's push the ball back a smidge so it doesn't get stuck in objects
	//if( %xNorm != 0 || %yNorm != 0 )
	// %posMod = vectorScale( vectorNormalize( %x SPC %y SPC 0 ), -0.5 );
	// echo( %posMOd );
	//else
	//	%posMod = "0 0 0";

	%item.setTransform( %obj.getPosition() SPC  "0 0 1" SPC %rot); // vectorAdd( %obj.getPosition(), %posMod ) SPC  "0 0 1" SPC %rot);
	%item.schedulePop();
	%item.isSportBall = 1;
	%item.isPuck = 1;

	// this is done to prevent leaks, so the object is deleted after the function is over.
	%obj.delete();//%obj.schedule( 0, delete );
}

//Say he has balls eheh
function PuckImage::onMount(%this,%obj)
{	
	%obj.hasSportBall = 1;
	%obj.hasPuck = 1;
	parent::onMount(%this,%obj);
}

function PuckImage::onUnMount(%this,%obj)
{
	%obj.hasSportBall = 0;
	%obj.hasPuck = 0;
}

datablock ItemData(PuckPickupItem)
{
	 // Basic Item Properties
	shapeFile = "Add-Ons/Item_Hockey/puck.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.05;
	emap = true;
	lifetime = 60000;

	//gui properties
	uiName = "";
	iconName = "nope"; //_________________________________________TODO
	doColorShift = true;
	colorShiftColor = "1.0 0.0 0.0 1.0";

	 // Dynamic properties defined by the scripts
	canDrop = true;
};



function PuckPickupItem::onPickup(%this,%obj,%col,%a)
{

	%image = %col.getMountedImage(0);
	//Echo("puck picked up");
	//echo(%image);
	if( isObject(%image) && %image.HockeyStick )
	{
		//echo("yes mounting puck");
		%col.hasPuck = true;
		%col.hasSportBall = true;
		%col.unmountimage( 0 );
		%col.mountImage(HockeyStickWPuckImage, 0 );
		%obj.delete();

	}
	else if( isObject(%image) && %image.GoalieStick )
	{
		//echo("yes mounting puck");
		%col.hasPuck = true;
		%col.hasSportBall = true;
		%col.unmountimage( 0 );
		%col.mountImage(GoalieStickWPuckImage, 0 );
		%obj.delete();

	}
}