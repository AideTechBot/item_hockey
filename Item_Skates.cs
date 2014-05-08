//ITEM_SKATES
datablock TSShapeConstructor(PlayerIceHockeyDts)
{
	baseShape  = "./content/player/m.dts";
	sequence0  = "./content/player/m_root.dsq root";

	sequence1  = "./content/player/m_back.dsq run";
	sequence2  = "./content/player/m_back.dsq walk";
	sequence3  = "./content/player/m_back.dsq back";
	sequence4  = "./content/player/m_side.dsq side";

	sequence5  = "./content/player/m_root.dsq crouch";
	sequence6  = "./content/player/m_root.dsq crouchRun";
	sequence7  = "./content/player/m_root.dsq crouchBack";
	sequence8  = "./content/player/m_root.dsq crouchSide";

	sequence9  = "./content/player/m_root.dsq look";
	sequence10 = "./content/player/m_headside.dsq headside";
	sequence11 = "./content/player/m_headup.dsq headUp";

	sequence12 = "./content/player/m_root.dsq jump";
	sequence13 = "./content/player/m_root.dsq standjump";
	sequence14 = "./content/player/m_sit.dsq fall";
	sequence15 = "./content/player/m_root.dsq land";

	sequence16 = "./content/player/m_armattack.dsq armAttack";
	sequence17 = "./content/player/m_armreadyleft.dsq armReadyLeft";
	sequence18 = "./content/player/m_armreadyright.dsq armReadyRight";
	sequence19 = "./content/player/m_armreadyboth.dsq armReadyBoth";
	sequence20 = "./content/player/m_spearready.dsq spearready";  
	sequence21 = "./content/player/m_spearthrow.dsq spearThrow";

	sequence22 = "./content/player/m_talk.dsq talk";  

	sequence23 = "./content/player/m_death1.dsq death1"; 
	
	sequence24 = "./content/player/m_shiftup.dsq shiftUp";
	sequence25 = "./content/player/m_shiftdown.dsq shiftDown";
	sequence26 = "./content/player/m_shiftaway.dsq shiftAway";
	sequence27 = "./content/player/m_shiftto.dsq shiftTo";
	sequence28 = "./content/player/m_shiftleft.dsq shiftLeft";
	sequence29 = "./content/player/m_shiftright.dsq shiftRight";
	sequence30 = "./content/player/m_rotcw.dsq rotCW";
	sequence31 = "./content/player/m_rotccw.dsq rotCCW";

	sequence32 = "./content/player/m_undo.dsq undo";
	sequence33 = "./content/player/m_plant.dsq plant";

	sequence34 = "./content/player/m_sit.dsq sit";

	sequence35 = "./content/player/m_wrench.dsq wrench";

  	sequence36 = "./content/player/m_activate.dsq activate";
    sequence37 = "./content/player/m_activate2.dsq activate2";

    sequence38 = "./content/player/m_leftrecoil.dsq leftrecoil";
}; 

datablock AudioProfile(Shave_Ice)
{
	filename = "./sounds/Hockey_ShaveIce.wav";
	description = AudioClosest3d;
	preload = false;
};
datablock AudioProfile(skate_quiet1)
{
	filename = "./sounds/skate_quiet1.wav";
	description = AudioClosest3d;
	preload = false;
};
datablock AudioProfile(skate_quiet2)
{
	filename = "./sounds/skate_quiet2.wav";
	description = AudioClosest3d;
	preload = false;
};
datablock AudioProfile(skate_loud1)
{
	filename = "./sounds/skate_loud1.wav";
	description = AudioClosest3d;
	preload = false;
};
datablock AudioProfile(skate_loud2)
{
	filename = "./sounds/skate_loud2.wav";
	description = AudioClosest3d;
	preload = false;
};


datablock ItemData(SkatesItem)
{
	 // Basic Item Properties
	shapeFile = "Add-Ons/Item_Hockey/skate.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui properties
	uiName = "Ice Skates";
	iconName = "./hockeyskates";
	doColorShift = false;
	colorShiftColor = "0.0 0.0 0.0 1.0";

	 // Dynamic properties defined by the scripts
	image = SkatesImage;
	canDrop = true;
};

datablock ShapeBaseImageData(SkatesImage)
{
   // Basic Item properties
   shapeFile = "Add-Ons/Item_Hockey/skate.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   rotation = eulerToMatrix( "0 -90 90" );

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = false;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = SkatesItem;
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
   colorShiftColor = SkatesItem.colorShiftColor;

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
datablock shapeBaseImageData( R_SkateEquippedImage )
{
	shapeFile = "./skate.dts";

	mountPoint = 3;
	offset = "0 0 0";
	rotation = eulerToMatrix( "0 0 90" );

	eyeOffset = "-9999 -9999 -9999";

	doColorShift = true;
	colorShiftColor = SkatesItem.colorShiftColor;

	className = "WeaponImage";
	armReady = false;
};

datablock shapeBaseImageData( L_SkateEquippedImage )
{
	shapeFile = "./skate.dts";

	mountPoint = 4;
	offset = "0 0 0";
	rotation = eulerToMatrix( "0 0 90" );

	eyeOffset = "-9999 -9999 -9999";

	doColorShift = true;
	colorShiftColor = SkatesItem.colorShiftColor;

	className = "WeaponImage";
	armReady = false;
};


datablock PlayerData(PlayerIceHockeyArmor : PlayerStandardArmor)
{
   shapeFile = "./content/player/m.dts";
   runForce = 1500;
   runEnergyDrain = 0;
   minRunEnergy = 0;
   maxForwardSpeed = 15;
   maxBackwardSpeed = 14;
   maxSideSpeed = 14;

   maxForwardCrouchSpeed = 0;
   maxBackwardCrouchSpeed = 0;
   maxSideCrouchSpeed = 0;
 
   maxDamage = 200;

   jumpForce = 0;
   jumpEnergyDrain = 0;
   minJumpEnergy = 0;
   jumpDelay = 0;

	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 0;

	uiName = "Ice hockey Player";
	showEnergyBar = false;

   runSurfaceAngle  = 55;
   jumpSurfaceAngle = 55;
};

//when he clicks with skates
function SkatesImage::onFire( %this, %obj, %slot )
{
	//you already have skates greedy boi
	if(%obj.hasgoalieskates)
	{
		%obj.client.bottomprint("<font:arial:21><color:ffffff>You already have goalie skates.",3);
		return;
	}
	//if he has skates then no
	if(%obj.hasskates)
	{
		%obj.client.bottomprint("<font:arial:21><color:ffffff>You removed your skates.",3);
		//unmount anything already on them
		%obj.unmountimage( 1 );
		%obj.unmountimage( 2 );
		%obj.hasskates = false;
		%obj.setdatablock(%obj.oldDatablock);
		return;
	}
	%obj.client.bottomprint("<font:arial:21><color:ffffff>You put on your skates.",3);
	//unmount anything already on them
	%obj.unmountimage( 1 );
	%obj.unmountimage( 2 );
	//mount skates 
	%obj.mountimage( R_SkateEquippedImage, 1 );
	%obj.mountimage( L_SkateEquippedImage, 2 );
	//save the old datablock before we cahnge it
	%obj.oldDatablock = %obj.getDatablock();
	//change the datablock
	%obj.setdatablock(PlayerIceHockeyArmor);
	//he has skates now
	%obj.hasskates = true;
	//this is his last position VVV and if fires the skate loop
	%obj.s_lastpos = getWords(%obj.getPosition(),0,1);
	%obj.skateLoop();

	//don't need this anymore
	//takes the skate out of the invetory
	//messageClient( %obj.client, 'MsgItemPickup', '', %obj.currTool, 0 );
	//%obj.tool[ %obj.currTool ] = 0;
	//serverCmdUnUseTool( %obj.client );
}