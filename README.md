# Satisfactory BluePrint Creator (SBPC)

A tool for programmatically creating Satisfactory blueprints, primarily for creating smooth and precisely aligned curves.

# References and Resources

- https://satisfactory.wiki.gg/wiki/Save_files
- https://github.com/moritz-h/satisfactory-3d-map/blob/edb8198744a66bad29f2a7722146241ed12c8e7c/libsave/include/SatisfactorySave/GameTypes/UE/Satisfactory/BlueprintTypes.h
- https://github.com/moritz-h/satisfactory-3d-map/blob/edb8198744a66bad29f2a7722146241ed12c8e7c/docs/SATISFACTORY_SAVE.md#decompressed-binary-data
- https://github.com/AnthorNet/SC-InteractiveMap/blob/8252711a2bb43af90d33e92c68886a57d0c98d07/src/SaveParser/Read.js

# Code Walkthrough

## Properties

Property parsing is separated out from the rest of the parsing code. Properties are pased into light weight record structs and stored in a list as objects.

All of the property structs can be found in `PropertyTypes.cs` while the code for reading and writing properties can be found in `PropertyBinaryReaderExtensions.cs` and `PropertyBinaryWriterExtensions`.