//ITEM_HOCKEYHELMET

datablock ItemData(HockeyHelmetItem)
{
	 // Basic Item Properties
	shapeFile = "Add-Ons/Item_Hockey/helmetItem.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui properties
	uiName = "Hockey Helmet";
	iconName = "";
	doColorShift = false;
	colorShiftColor = "0.0 0.0 0.0 1.0";

	 // Dynamic properties defined by the scripts
	image = HockeyHelmetImage;
	canDrop = true;
};

datablock ShapeBaseImageData(HockeyHelmetImage)
{
   // Basic Item properties
   shapeFile = "Add-Ons/Item_Hockey/helmetItem.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   rotation = eulerToMatrix( "0 0 0" );

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
datablock shapeBaseImageData( HockeyHelmetEquippedImage )
{
	shapeFile = "./helmetMount.dts";

	mountPoint = 5;
	offset = "0 0 0";
	rotation = eulerToMatrix( "0 0 0" );

	eyeOffset = "-9999 -9999 -9999";

	doColorShift = true;
	colorShiftColor = HockeyHelmetItem.colorShiftColor;

	className = "WeaponImage";
	armReady = false;
};

//when he clicks with skates
function HockeyHelmetImage::onFire( %this, %obj, %slot )
{
	if(%obj.hashelmet)
	{
		%obj.client.bottomprint("<font:arial:21><color:ffffff>You removed your helmet.",3);
		//unmount anything already on them
		%obj.unmountimage( 3 );
		%obj.hashelmet = false;
		return;
	}
	%obj.client.bottomprint("<font:arial:21><color:ffffff>You put on your helmet.",3);
	//unmount anything already on them
	%obj.unmountimage( 32 );

	%obj.mountimage( HockeyHelmetEquippedImage, 3 );
	%obj.hashelmet = true;

	//hide all hats
	%obj.hideNode(bicorn); %obj.hideNode(cophat); %obj.hideNode(triplume); %obj.hideNode(flarehelmet); %obj.hideNode(helmet); %obj.hideNode(knithat); %obj.hideNode(plume); %obj.hideNode(pointyhelmet); %obj.hideNode(scouthat); %obj.hideNode(septplume); %obj.hideNode(visor);
	
	//don't need this anymore
	//messageClient( %obj.client, 'MsgItemPickup', '', %obj.currTool, 0 );
	//%obj.tool[ %obj.currTool ] = 0;
	//serverCmdUnUseTool( %obj.client );
}