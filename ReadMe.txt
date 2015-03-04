Adding new tests:
If you need new tests under a new feature area, follow the existing structure set by the UnitTests.
There should be a .sln file in this directory which has your AdHoc project and dependent projects referenced.
The AdHoc project is for development and on-the-fly testing of the feature.
The AdHoc project is created using VisualStudio and can be built from within Visual Studio.  However, it references assemblies
from within the enlistment.
When it's time to automate the tests, create the non-AdHoc version of your project that can be built in razzle.

Adding to existing projects:
First, prototype your test using the AdHoc version of the project you wish to add tests to.  Once they are working, add
the code to the existing projects.

Notes:
There is zero duplication of code files - the AdHoc projects reference the same code files as the "razzle" versions.