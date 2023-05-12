using UnityEngine.UIElements;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{

    public enum MapLevel
    {
        Star,
        Planet,
        Moon,
        SpaceStation
    }
    private MapLevel currentMapLevel;
    public SolarSystemManager solarSystemManager;
    public float gridCreationDelay = 0.001f;

    public VisualElement root;
    public VisualElement gridContainer;
    public VisualElement[,] gridElements;

    public int gridSize;
    public float gridElementSize;

    public int poolSize = 10000;
    private Queue<VisualElement> gridElementPool;

    private void Awake()
    {
        gridElementPool = new Queue<VisualElement>(poolSize);
        InitializeGridElementPool(poolSize);
    }
    public void OpenMap()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        gridContainer = root.Q<VisualElement>("gridContainer");
       
        
    }
    private void InitializeGridElementPool(int size)
    {
        for (int i = 0; i < size; i++)
        {
            VisualElement gridElement = CreateGridElement();
            gridElementPool.Enqueue(gridElement);
        }
    }
    private VisualElement GetGridElementFromPool()
    {
        if (gridElementPool.Count == 0)
        {
            InitializeGridElementPool(poolSize);
        }

        return gridElementPool.Dequeue();
    }
    private void ReturnGridElementToPool(VisualElement gridElement)
    {
        gridElementPool.Enqueue(gridElement);
    }
    private VisualElement CreateGridElement()
    {
        VisualElement gridElement = new VisualElement();
        gridElement.style.backgroundColor = Color.gray;
        gridElement.style.borderLeftWidth = 1;
        gridElement.style.borderTopWidth = 1;
        gridElement.style.borderTopColor = Color.black;
        gridElement.style.borderBottomColor = Color.black;
        gridElement.style.borderLeftColor = Color.black;
        gridElement.style.borderRightColor = Color.black;
        return gridElement;
    }




    private IEnumerator CreateGrid(int gridSize, float gridElementSize, MapLevel mapLevel, CelestialObject selectedObject)
    {
        gridElements = new VisualElement[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                VisualElement gridElement = GetGridElementFromPool();

                // Set the grid element properties
                gridElement.style.width = gridElementSize;
                gridElement.style.height = gridElementSize;
                gridElement.style.position = Position.Absolute;
                gridElement.style.left = i * gridElementSize;
                gridElement.style.top = j * gridElementSize;

                int x = i;
                int y = j;
                gridElement.RegisterCallback<ClickEvent>(evt => OnGridElementClicked(x, y));
                gridElements[i, j] = gridElement;
                gridContainer.Add(gridElement);

                yield return new WaitForSeconds(gridCreationDelay);
            }
        }

        ShowSelectedMap(selectedObject);
    }

    public void PlaceSolarSystemObject(CelestialObject obj)
    {
        if (obj.Position.x >= 0 && obj.Position.x < gridSize && obj.Position.y >= 0 && obj.Position.y < gridSize)
        {
            gridElements[obj.Position.x, obj.Position.y].style.backgroundColor = obj.color;
            if(obj.icon != null)
            {
                gridElements[obj.Position.x, obj.Position.y].style.backgroundImage = obj.icon;
            }
            gridElements[obj.Position.x, obj.Position.y].userData = obj;
        }

        
    }

    private void OnGridElementClicked(int x, int y)
    {
        Debug.Log($"Grid element clicked at: ({x}, {y})");

        switch (currentMapLevel)
        {
            case MapLevel.Star:
                // Find the planet or station at the clicked position and show the corresponding map
                // ShowPlanetMap(planet) or ShowSpaceStationMap(spaceStation)
                break;
            case MapLevel.Planet:
                // Find the moon or station at the clicked position and show the corresponding map
                // ShowMoonMap(moon) or ShowSpaceStationMap(spaceStation)
                break;
            case MapLevel.Moon:
                // Find the station at the clicked position and show the corresponding map
                // ShowSpaceStationMap(spaceStation)
                break;
            case MapLevel.SpaceStation:
                // Handle location clicks
                break;
        }
    }
    public void ShowStarMap()
    {
        ClearGrid();
        
        currentMapLevel = MapLevel.Star;
        gridSize = 50; // Adjust gridSize and gridElementSize based on the scale you want
        gridElementSize = 10;

        StartCoroutine(CreateGrid(gridSize, gridElementSize, MapLevel.Star, solarSystemManager.Star));
    }
    public void ShowSelectedMap(CelestialObject selectedObject)
    {
        
        if(selectedObject != null) 
        {
            if(selectedObject is Star star)
            {
                star.Position = new Vector2Int(gridSize / 2, gridSize / 2);
                PlaceSolarSystemObject(star);
                // Populate the grid with the solar system objects (planets and stations)
                float scaleFactor = gridElementSize/10;
                foreach (Planet planet in star.Planets)
                {
                    // Calculate the planet's grid position based on its distance from the star
                    // Note: You might need to adjust the position calculation based on your specific scaling factors
                    double rotationInRadians = planet.Rotation * Math.PI / 180;

                    int gridCells = (int)(planet.DistanceFromParent / scaleFactor);


                    // Calculate the x and y positions
                    int x = (int)(gridSize / 2 + (gridCells * Math.Cos(rotationInRadians)));
                    int y = (int)(gridSize / 2 + (gridCells * Math.Sin(rotationInRadians)));

                    planet.Position.x = x; planet.Position.y = y;
                    // Place the planet in the grid
                    PlaceSolarSystemObject(planet);
                }
            }
        }
        
        

        

        //foreach (CelestialObject station in star.stations)
        //{
        //    // Calculate the station's grid position based on its distance from the star
        //    // Note: You might need to adjust the position calculation based on your specific scaling factors
        //    int x = (int)(gridSize / 2 + (station.DistanceFromParent / (gridSize * gridElementSize)));
        //    int y = gridSize / 2;
        //    station.Position.x = x; station.Position.y = y;
        //    // Place the station in the grid
        //    PlaceSolarSystemObject(station);
        //}
    }

    public void ShowPlanetMap(Planet planet)
    {
        ClearGrid();
        currentMapLevel = MapLevel.Planet;
        gridSize = 50;
        gridElementSize = 20;
        StartCoroutine(CreateGrid(gridSize, gridElementSize, MapLevel.Planet,planet));

        // Populate the grid with the solar system objects (moons and stations)
        // ...
    }

    public void ShowMoonMap(Moon moon)
    {
        ClearGrid();
        currentMapLevel = MapLevel.Moon;
        gridSize = 25;
        gridElementSize = 40;
        StartCoroutine(CreateGrid(gridSize, gridElementSize, MapLevel.Moon, moon));

        // Populate the grid with the solar system objects (stations)
        // ...
    }

    public void ShowSpaceStationMap(CelestialObject spaceStation)
    {
        ClearGrid();
        currentMapLevel = MapLevel.SpaceStation;
        gridSize = 10;
        gridElementSize = 100;
        StartCoroutine(CreateGrid(gridSize, gridElementSize, MapLevel.SpaceStation, spaceStation));

        // Populate the grid with the locations
        // ...
    }
    public void ClearGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // Set the grid element color to the default (transparent)
                if (gridElements != null && gridElements[x, y] != null)
                {
                    gridElements[x, y].style.backgroundColor = new Color(0, 0, 0, 0);
                    gridElements[x, y].userData = null;
                    ReturnGridElementToPool(gridElements[x, y]);
                    gridContainer.Remove(gridElements[x, y]);
                }
            }
        }
    }
    public void UpdateSolarSystemObjectPosition(CelestialObject obj, double distanceFromParent, double rotation)
    {
        // Calculate the new grid position based on the distance and rotation
        int x = (int)(gridSize / 2 + distanceFromParent * Math.Cos(rotation) / (gridSize * gridElementSize));
        int y = (int)(gridSize / 2 + distanceFromParent * Math.Sin(rotation) / (gridSize * gridElementSize));

        // Clear the previous position and place the object at the new position
        ClearGrid();

        if (obj is Planet planet)
        {
            PlaceSolarSystemObject(planet);
        }        
        else if (obj is Moon moon)
        {
            PlaceSolarSystemObject(moon);
        }
        else if (obj is CelestialObject station)
        {
            PlaceSolarSystemObject(station);
        }
    }

}
