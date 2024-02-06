# BezierAirfoilDesigner

An Open-Source-App for converting airfoil coordinates into Bézier curves.

## Which problem is the app supposed to solve?

Anyone who has tried to model a plane in CAD knows the problem of importing airfoils.
First, you have to find a way to input the coordinates into the software. Once you have solved that, you need to convert the points into a curve, usually a spline is used to connect all the points.
If you want to model the upper and lower surfaces of the wing separately, you have to split the curve into upper and lower parts. This often creates a small discontinuity at the leading edge, the more coordinates you have, the smaller this tip becomes.
You also need to consider how many coordinates the profile should have. Too few, and the deviation from the original becomes too large; too many, and the profile becomes overdefined, causing issues in further design steps.
Even if you have found a good process, the spline curve is never completely smooth, and there are always small waves. These waves are then transferred to the final surfaces as well.

![image](https://github.com/marc-frank/BezierAirfoilDesigner/assets/74321912/54279ddb-6641-4c81-8012-d313f4129cc8)

If you also want to set a specific trailing edge thickness, you have to adjust all the coordinates or rotate them around the leading edge. Overall, the process is not very straightforward, and in my opinion, the results could be improved.

## What does the BezierAirfoilDesigner do?

The BezierAirfoilDesigner allows you to import regular .dat airfoil coordinates into the program.
Then, you can either manually, automatically, or through a combination of both,
fit two Bézier curves to the loaded airfoil – one for the upper and one for the lower surface.

![image](https://github.com/marc-frank/BezierAirfoilDesigner/assets/74321912/a2a8b32e-8a54-45f0-8179-91be5475b3d1)

![image](https://github.com/marc-frank/BezierAirfoilDesigner/assets/74321912/6a6813b4-0fb8-4e57-b56b-1c57cc8071ff)

To do this, you can move the control points either using the mouse or enter coordinates directly into the corresponding table. Under the "search" function, a recursive algorithm can be used to try out different positions for the control points to reduce the error (difference from the reference airfoil). The "Search Top" and "Search Bottom" functions continue searching for new points until the improvement is under 5%.

Initially, three other positions per control point are tested. If there is no better position found, the number of tested points within the same "search radius" (only vertical) is increased by 1.

With "auto search," the degree of the curves is increased by 1 (adding a control point) after each search process for the upper and lower curves, without altering the shape of the curve. This process is repeated until the error is under 0.075.

Depending on the quality and complexity of the reference airfoil, the automatic adjustment can take between 10 minutes and 3 hours. For very poor-quality profiles, the termination condition might never be reached, in which case there is a stop button to abort the current search.

## When the search is completed...

...you can export the control points as .bez.dat files.
These files have the same structure as normal .dat files, and thanks to the same file extension, you can import them into CAD software just like normal .dat files. The curves can then be drawn using a "control point spline" (in Fusion 360 or similar curve types in other software).

Currently, there is a limit of 6 control points when implementing Bézier curves using the "control point spline" in Fusion 360 (there is a workaround to use up to 10 control points). With more control points, the curves no longer match. I'm not sure if this is a bug in Fusion 360 or related to the curve type they are using. I have already addressed this issue, so there might be changes in the future.

Solidworks and Onshape have no problem drawing the correct curves.
Rhino7 can do up to degree 11.

## Results in CAD:

The airfoils drawn from the control points are already divided into upper and lower surfaces by default, so you don't need to do this separately. The curvature distribution no longer shows waves on the surface.

![image](https://github.com/marc-frank/BezierAirfoilDesigner/assets/74321912/f76aee4a-2157-4c97-9675-85973ead7c36)

You can easily set the trailing edge thickness by adding another dimension.

![image](https://github.com/marc-frank/BezierAirfoilDesigner/assets/74321912/1a463613-a831-4649-a029-9e1d8bee4414)

## Another use case

The program can be used not only to simplify and improve processes in CAD but also to smooth existing profiles, potentially improving the performance of the airfoil.
The resulting coordinates are naturally spaced based on the curvature. Using a continous bezier curve creates a very smooth curve, without waves or bumps, such as a simple spline interpolation would do.
This also leads to very nice velocity distributions.

Before:

![image](https://github.com/marc-frank/BezierAirfoilDesigner/assets/74321912/bbd6585d-2b28-46d2-ab88-058bba7e1105)

After:

![image](https://github.com/marc-frank/BezierAirfoilDesigner/assets/74321912/1d0855e4-29d8-4c3b-8c64-3e96af7519f1)

Polars are similar, maybe a tiny bit better.

![image](https://github.com/marc-frank/BezierAirfoilDesigner/assets/74321912/775eb0f2-9e71-4135-891d-b04f918047e3)

## Miscellaneous

In addition to the .bez.dat format, you can also save the control points as .bez files. This allows the representation of shapes where the beginning and end of the curves are not identical and do not lie between 0 and 1. This might be useful for creating wing outlines.

Furthermore, you can export the coordinates of the Bézier curves as .dat files. However, it is essential to consider the program in which you want to use the profiles further. The Profile Editor by Frank Ranis accepts a maximum of 450 points, and xflr5 has a limit of 300 points.

If it's crucial that all points on both curves have the same X-coordinates, you should ensure that all control points have the same X-coordinates as well.

## Installation

For Windows, simply download the appropriate .exe file from GitHub under Releases.

Before version v0.9, I selected "Framework-dependent" in the release settings. If you want to try earlier versions, you'll need to download .NET 7 as well.

The app was created in Visual Studio as a Winforms project using C#.
I'm not sure if it runs under Linux with Wine as it hasn't worked in previous tests.
However, it runs in a VM under MacOS.

---

I would appreciate feedback and suggestions for further functionality.
Bugs in the program and minor feature requests can be reported under Issues.
If you want to fix them yourself, you can create a Pull request.

Best regards,
Marc
