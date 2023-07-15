# BezierAirfoilDesigner

A tool to manipulate control points of bezier curves.  
Main intended use is to generate .dat airfoil files which can be used to design subscale airplane wings.  

![image](https://github.com/Marc-Frank/BezierAirfoilDesigner/assets/74321912/290aa1ed-7230-4ddc-b359-40222af274c7)


Exporting the airfoil as a .bez.dat makes handling them in CAD easier.  
This saves the control points instead of points on the curve.  
Using these in CAD creates higher quality surfaces with less computation and constraints needed.  
Setting the trailing edge gap is as easy as adding one more dimension.  

![image](https://github.com/Marc-Frank/BezierAirfoilDesigner/assets/74321912/061d33eb-45d7-49de-b4a1-30434a2ff780)

The resulting coordinates are naturally spaced based on the curvature.  
Using a continous bezier curve creates a very smooth curve, without waves or bumps, such as a simple spline interpolation would do.  
This also leads to very nice velocity distributions.  

![image](https://github.com/Marc-Frank/BezierAirfoilDesigner/assets/74321912/b572f173-d2e7-4fdb-9550-56a171111f51)

