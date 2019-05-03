using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*------------To Do------------
 - Have the tile change based on its parent which will be assigned when getting dragged over
 */

public enum TypeOfTile
{
    StartTube,
    EndTube,
    Tube,
    Accessible,
    Inaccessible
}

namespace Room3
{
    [RequireComponent( typeof(Image))]
    public class Tile : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
    {
        public event Action<Tile> OnMouseHoover;
        public TypeOfTile CurrentTileType;
        private TypeOfTile initialTileType;
        public Image image;
        private Color32 initialColour;
        public bool North => NorthTile != null;
        public bool South => SouthTile != null;
        public bool East => EastTile != null;
        public bool West => WestTile != null;
        public Tile NorthTile;
        public Tile SouthTile;
        public Tile WestTile;
        public Tile EastTile;

        [SerializeField] private TileSettings tileSettings;

        private void Awake()
        {
            image = GetComponent<Image>();
        }
        private void Start()
        {
            initialTileType = CurrentTileType;
        }
        public void CheckState()
        {
            //this method should be called alongside the check OnInvoke as tubes need to change depending on their adjacent tubes
            switch (CurrentTileType)
            {
                case TypeOfTile.StartTube:
                    image.sprite = tileSettings.StartPointSprite;
                    break;
                case TypeOfTile.EndTube:
                    image.sprite = tileSettings.EndPointSprite;
                    break;
                case TypeOfTile.Tube:
                    if (West && North)
                    {
                        image.sprite = tileSettings.CornerWestToNorthTubeSprite;
                        break;
                    }
                    if (West && South)
                    {
                        image.sprite = tileSettings.CornerWestToSouthTubeSprite;
                        break;
                    }
                    if (East && South)
                    {
                        image.sprite = tileSettings.CornerEastToSouthTubeSprite;
                        break;
                    }
                    if (East && North)
                    {
                        image.sprite = tileSettings.CornerEastToNorthTubeSprite;
                        break;
                    }
                    if (West || East)
                    {
                        image.sprite = tileSettings.HorizontalTubeSprite;
                        break;
                    }
                    if (North || South)
                    {
                        image.sprite = tileSettings.VerticalTubeSprite;
                        break;
                    }
                    break;
                case TypeOfTile.Accessible:
                    image.sprite = tileSettings.AccessibleSprite;
                    break;
                case TypeOfTile.Inaccessible:
                    image.sprite = tileSettings.InAccessibleSprite;
                    break;
                default:
                    image.sprite = tileSettings.InAccessibleSprite;
                    break;
            }
        }




        public void ResetTube()
        {
            // Make all the tubes apart from the start and end tube, accessibles. 
            // if the tube was finished send a message that it is incomplete again
            if (North)
            {
                NorthTile.SouthTile = null;
                NorthTile = null;
            }
            if (East)
            {
                EastTile.WestTile = null;
                EastTile = null;
            }
            if (South)
            {
                SouthTile.NorthTile = null;
                SouthTile = null;
            }
            if (West)
            {
                WestTile.EastTile = null;
                WestTile = null;
            }
            CurrentTileType = initialTileType;
            image.color = initialColour;
            CheckState();
        }


        public void ChangeState(Color32 color, bool overrideInitialColor = false, bool shouldBecomeTube = true)
        {
            image.color = color;

            if (shouldBecomeTube)
            {
                if (initialTileType != TypeOfTile.StartTube && initialTileType != TypeOfTile.EndTube)
                {
                    CurrentTileType = TypeOfTile.Tube;
                }
            }

            if (overrideInitialColor)
            {
                initialColour = color;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (CurrentTileType == TypeOfTile.StartTube)
            {
                OnMouseHoover?.Invoke(this);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Input.GetMouseButton(0))
            {
                OnMouseHoover?.Invoke(this);
            }
        }
    }
}
