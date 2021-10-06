# ImageResizeProxy

This project is a demo that shows how to use this Azure Function (3.1) to dynamically resize images that are held in an Azure Storage container.  

There are several query string switches that can be used.  Here are all of them currently

```
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?size=small
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?size=medium
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?size=hero
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?w=200
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?h=300
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?w=400&h=300
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?output=png
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?output=gif
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?mode=stretch
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?mode=boxpad
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?mode=pad
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?mode=max
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?mode=min
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?mode=crop
// all together
https://function-endpoint.azurewebsites.net/api/resizeimage/someimage.jpg?w=400&h=300&output=png&mode=stretch
```

To run locally, rename the example.local.settings.json file to local.settings.json and update the settings to fit your environment.

To run in Azure, simply publish the project to a new Azure Function and create the following Application Settings in the Azure Portal:

1. AzureContainer - name of the container
2. ImageResizer:HeroSize - 1440x620
3. ImageResizer:MediumSize - 400x400
4. ImageResizer:SmallSize - 200x200
5. ClientCache:MaxAge - 30.00:00:00

And add in a Connection String called "AzureStorage" that cooresponds with your storage account.

Happy Resizing.
