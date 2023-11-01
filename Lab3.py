#import modules
import arcpy
import sys
import os
import csv
import plotly
import plotly.graph_objects as go

# allow overwrite
arcpy.env.overwriteOutput = True

# set directory
rootDir = r"C:\\Data\\IGME_386_lab_COVID Hurricane Data Processing_datasets\\"

print ("CSV Into Shapefile...")

#define projection this was wonky so i just added wgs_1984.prj to rootdir
#WGS_84_Info = r'GEOGCS["GCS_WGS_1984",DATUM["D_WGS_1984",SPHEROID["WGS_1984",6378137.0,298.257223563]],PRIMEM["Greenwich",0.0],UNIT["Degree",0.0174532925199433]]'
#f = open(root_directory + "WGS_84.prj", "w")
#f.write(WGS_84_Info)
#f.close()

# set csv directory
file_in = rootDir + 'time_series_covid19_confirmed_global.csv'

# define csv fields
csv_lat = "Lat"
csv_long = "Long"
csv_location = "Country"
csv_date = "9_11_2020"

# generate shapefile
print ("Creating Shapefile...")

# set prj file directory
prj = rootDir  + "wgs_84.prj"

# create shapefile
shapefile = arcpy.CreateFeatureclass_management(rootDir, "covid.shp", "POINT","","DISABLED","DISABLED", prj)
print ("Shapefile Created")

print ("Define Shapefile Fields")
# define fields in shapefile
recordID_Name = "record_id"
recordID_Type = "LONG"
recordID_Precision = 9

cases_Name = "cases"
cases_Type = "LONG"
cases_Precision = 9

location_Name = "country"
location_Type = "TEXT"
location_Length = 255

# create fields in shapefile
arcpy.AddField_management(shapefile, recordID_Name, recordID_Type, recordID_Precision)
arcpy.AddField_management(shapefile, cases_Name, cases_Type, cases_Precision)
arcpy.AddField_management(shapefile, location_Name, location_Type, field_length = location_Length)

# open csv
with open(file_in) as csvfile:
    # get data
    data = csv.DictReader(csvfile)
    
    # track id
    currentID = 0

    # get each row
    for row in data:
        #inspect the coordinate values
        print("Coord:" + row[csv_lat] + "," + row[csv_long])
        try:
            # shapefile field names
            fields = ['SHAPE@XY', cases_Name, location_Name, recordID_Name]
           
            # read data with cursor
            cursor = arcpy.da.InsertCursor(shapefile, fields)
            
            #convert lat and long into float values
            xy = (float(row[csv_long]), float(row[csv_lat]))
            
            # insert csv data
            cursor.insertRow((xy, float(row[csv_date]), row[csv_location], currentID))
            # next id
            currentID += 1

        except Exception:
            #catch errors
            e = sys.exc_info()[1]
            print(e.args[0])

#delete cursor
del cursor

# hurricane shapefile
hurricane_shp = rootDir + 'hurricane.shp'

# name of combined file
selected = 'hurricane_cases'

# get intersected data
impacted = arcpy.SelectLayerByLocation_management('covid.shp', 'INTERSECT', hurricane_shp)

#see if anything was found
matchcount = int(arcpy.GetCount_management(impacted)[0]) 

if matchcount == 0:
    print('no features matched spatial and attribute criteria')
else:
    #see if the file exists
    if arcpy.Exists(selected):
        arcpy.Delete_management(selected)
        print ("deleted existing feature class")

    arcpy.CopyFeatures_management(impacted, selected)
    print('{0} tracts that matched criteria written to {1}'.format(matchcount, selected))

# get sum of cases
statsFields = [["cases", "SUM"]]
arcpy.analysis.Statistics(impacted,"stats",statsFields)

# plotly
covid_counts = []
covid_country = []

with arcpy.da.SearchCursor(selected, (location_Name, cases_Name)) as covid_cursor:
    for row in covid_cursor:
        # add dates to plot if > 10000 covid cases found
        if (int(row[1])) >= 10000:
            covid_counts.append(row[1])
            covid_country.append(row[0]) 

# display graph
fig = go.Figure(
    data=[go.Bar(x=covid_country, y=covid_counts)],
    layout=go.Layout(
        title=go.layout.Title(text="Covid Countries Hit By The Hurricane")
    )
)

fig.write_html(rootDir + 'covid_and_hurricane.html', auto_open=True)

print ("Finished")