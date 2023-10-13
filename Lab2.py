#import modules
import arcpy
import sys
import os
import csv

# To allow overwriting the outputs change the overwrite option to true.
arcpy.env.overwriteOutput = True

#the directory where data will be processed
root_directory = "c:\\Data\\csv\\"

#setup the output feature class
print ("creating new feature class")

#need to define a projection to display correctly
prj = root_directory + "wgs_84.prj"

#create the shape file using ArcPy - https://pro.arcgis.com/en/pro-app/latest/tool-reference/data-management/create-feature-class.htm
Output_FC = arcpy.CreateFeatureclass_management(root_directory, "earthquakes.shp", "POINT","","DISABLED","DISABLED", prj)
print ("finished creating new feature class")

#the CSV file coming in, the contents will be written into the shapefile
file_in = root_directory + 'earthquakes.csv'

#define the fields from the CSV that will be accessed
LAT_field = "latitude"
LON_field = "longitude"
# OVERDUE_field = "OVERDUE"
# SR_TYPE_field = "SR TYPE"

#define fields in shapefile
fieldName_RecordId = "record_id"
fieldPrecision = 9
fieldAlias = "refcode"

# fieldName_Overdue = "overdue"
fieldName_Depth = "depth"

fieldName_Magnitude = "mag"

# fieldName_sr = "type"
fieldName_QuakePlace = "place"
fieldLength = 255

#create fields in the new shapefile - https://pro.arcgis.com/en/pro-app/latest/tool-reference/data-management/add-field.htm
arcpy.AddField_management(Output_FC, fieldName_RecordId, "LONG", fieldPrecision, field_alias=fieldAlias, field_is_nullable="NULLABLE")
# arcpy.AddField_management(Output_FC, fieldName_Overdue, "DOUBLE", field_length=fieldLength)
# arcpy.AddField_management(Output_FC, fieldName_sr, "TEXT", field_length=fieldLength)
arcpy.AddField_management(Output_FC, fieldName_Depth, "DOUBLE", field_length=fieldLength)
arcpy.AddField_management(Output_FC, fieldName_Magnitude, "DOUBLE", field_length=fieldLength)
arcpy.AddField_management(Output_FC, fieldName_QuakePlace, "TEXT", field_length=fieldLength)

#open the CSV for reading its contents
with open(file_in) as csvfile:
    #read the CSV as a dict - i.e, key:value pairs - https://docs.python.org/3/library/csv.html#csv.DictReader
    current_data = csv.DictReader(csvfile)
    #seed the unqiue ID value, start at 0
    rowidval = 0
    #begin for loop of the CSV dict object and access of the CSV fields defined previosuly starting on line 28
    for row in current_data:
        #inspect the coordinate values
        print("Coord:" + row[LAT_field] + "," + row[LON_field])
        try:
            #the fields in the shape file that were 
            fields = ['SHAPE@XY', fieldName_RecordId, fieldName_Depth, fieldName_Magnitude, fieldName_QuakePlace]
            #a cursor is used to read/write data to the shapefile
            #this code will access the shapefile to beging editing - https://pro.arcgis.com/en/pro-app/latest/arcpy/data-access/insertcursor-class.htm
            cursor = arcpy.da.InsertCursor(Output_FC, fields)
            #convert the lat and long from the CSV into a list of float values
            xy = (float(row[LON_field]), float(row[LAT_field]))
            #write a new row of data into the shapefile using the contents of the CSV that is currently in the loop
            #the order of data being writen has to be the same as the varibale fields defined around line 62
            cursor.insertRow((xy, rowidval, float(row[fieldName_Depth]), float(row[fieldName_Magnitude]), row[fieldName_QuakePlace]))
            #increment the unqiue ID value by 1 to get ready for the next iteration of the loop
            rowidval += 1
        except Exception:
            #catch any errors that might happen
            e = sys.exc_info()[1]
            print(e.args[0])
#delete cursor object from memory so the final shapefile can be accessed when the code is finished
del cursor
print ("done")