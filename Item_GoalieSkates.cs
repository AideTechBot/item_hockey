//ITEM_GOALIESKATES

datablock TSShapeConstructor(PlayerGoalieDts)
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

datablock ItemData(GoalieSkatesItem)
{
	 // Basic Item Properties
	shapeFile = "Add-Ons/Item_Hockey/skate.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui properties
	uiName = "Goalie Skates";
	iconName = "./hockeyskates";
	doColorShift = false;
	colorShiftColor = "0.0 0.0 0.0 1.0";

	 // Dynamic properties defined by the scripts
	image = GoalieSkatesImage;
	canDrop = true;
};

datablock ShapeBaseImageData(GoalieSkatesImage)
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
   item = GoalieSkatesItem;
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
   colorShiftColor = GoalieSkatesItem.colorShiftColor;

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
datablock PlayerData(PlayerGoalieArmor : PlayerStandardArmor)
{
   shapeFile = "./content/player/m.dts";
   runForce = 1600;
   runEnergyDrain = 0;
   minRunEnergy = 0;
   maxForwardSpeed = 7;
   maxBackwardSpeed = 6;
   maxSideSpeed = 6;

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

	uiName = "Goalie Player";
	showEnergyBar = false;

   runSurfaceAngle  = 55;
   jumpSurfaceAngle = 55;
};

datablock shapeBaseImageData( R_GoalieSkateEquippedImage )
{
	shapeFile = "./gskateright.dts";

	mountPoint = 3;
	offset = "0 0 0";
	rotation = eulerToMatrix( "0 0 0" );

	eyeOffset = "-9999 -9999 -9999";

	doColorShift = true;
	colorShiftColor = SkatesItem.colorShiftColor;

	className = "WeaponImage";
	armReady = false;
};

datablock shapeBaseImageData( L_GoalieSkateEquippedImage )
{
	shapeFile = "./gskateleft.dts";

	mountPoint = 4;
	offset = "0 0 0";
	rotation = eulerToMatrix( "0 0 0" );

	eyeOffset = "-9999 -9999 -9999";

	doColorShift = true;
	colorShiftColor = SkatesItem.colorShiftColor;

	className = "WeaponImage";
	armReady = false;
};


//when he clicks with skates
function GoalieSkatesImage::onFire( %this, %obj, %slot )
{
	if(%obj.hasskates)
	{
		%obj.client.bottomprint("<font:arial:21><color:ffffff>You already have normal skates.",3);
		return;
	}
	//if he has skates then no
	if(%obj.hasgoalieskates)
	{
		%obj.client.bottomprint("<font:arial:21><color:ffffff>You removed your goalie skates.",3);
		//unmount anything already on them
		%obj.unmountimage( 1 );
		%obj.unmountimage( 2 );
		%obj.hasgoalieskates = false;
		%obj.setdatablock(%obj.oldDatablock);
		%obj.setScale("1 1 1");
		return;
	}
	%obj.client.bottomprint("<font:arial:21><color:ffffff>You put on your goalie skates.",3);
	//unmount anything already on them
	%obj.unmountimage( 1 );
	%obj.unmountimage( 2 );
	//mount skates 
	%obj.mountimage( R_GoalieSkateEquippedImage, 1 );
	%obj.mountimage( L_GoalieSkateEquippedImage, 2 );
	//save the old datablock before we cahnge it
	%obj.oldDatablock = %obj.getDatablock();
	//change the datablock and scale
	%obj.setdatablock(PlayerGoalieArmor);
	%obj.setScale("1.5 1 1");
	//he has skates now
	%obj.hasgoalieskates = true;
	//this is his last position VVV and if fires the skate loop
	%obj.s_lastpos = getWords(%obj.getPosition(),0,1);
	%obj.skateLoop();

	//don't need this anymore
	
	//takes the skate out of the invetory
	//messageClient( %obj.client, 'MsgItemPickup', '', %obj.currTool, 0 );
	//%obj.tool[ %obj.currTool ] = 0;
	//serverCmdUnUseTool( %obj.client );
}