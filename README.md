# sbpc

A tool for programmatically creating Satisfactory blueprints, primarily for creating smooth and precisely aligned curves.

# Code Walkthrough

## Properties

Property parsing is separated out from the rest of the parsing code. Properties are pased into light weight record structs and stored in a list as objects.

All of the property structs can be found in `PropertyTypes.cs` while the code for reading and writing properties can be found in `PropertyBinaryReaderExtensions.cs` and `PropertyBinaryWriterExtensions`.