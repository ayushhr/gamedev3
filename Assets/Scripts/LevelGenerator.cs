using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int[,] levelMap =
             {
             {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
             {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
             {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
             {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
             {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
             {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
             {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
             {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
             {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
             {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
             {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
             {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
             {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
             {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
             {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
             };

        var upperX = levelMap.GetUpperBound(0);
        Debug.Log($"UpperX: {upperX}");
        var upperY = levelMap.GetUpperBound(1);
        Debug.Log($"upperY: {upperY}");

        int[,] stateMap = new int[upperX+1, upperY+1];

        float posY = 3f;
        int typeState = 0;

        var prevTypeId = 0;
        var prevTypeState = 0;

        for (int i=0; i<= upperX; i++)
        {
            float posX = -5f;
            for (int j = 0; j<= upperY; j++)
            {
                var wallTypeId = levelMap[i, j];
                var upperTypeId = i != 0 ? levelMap[i - 1, j] : -1;
                var upperTypeState = i != 0 ? stateMap[i - 1, j] : -1;


                //calcutate type state
                switch (wallTypeId)
                {
                    case 1:
                        if (j == 0)
                        {
                            typeState = i != 0 ? 3 : 0;
                        }
                        else
                        {
                            if (prevTypeId == 2)
                            {
                                if (upperTypeId == 2)
                                {
                                    typeState = 2;
                                }
                                else
                                {
                                    typeState = 1;
                                }
                            }
                        }
                        break;
                    case 2:
                        if (j==0)
                        {
                            typeState = upperTypeId == 1 || upperTypeId == 2 ? 1 : 0;
                        }
                        else
                        {
                            typeState = (prevTypeId == 1 || prevTypeId == 2 || prevTypeId == 7) ? 0 : 1;
                        }

                        break;
                    case 3:
                        if (prevTypeId == 4)
                        {
                            if (prevTypeState == 1)
                            {
                                typeState = (upperTypeId == 4) ? 3 : 0;
                            }
                            else if (prevTypeState == 0 && upperTypeId == 4)
                            {
                                typeState = upperTypeState == 1 ? 2 : 1;
                            }
                            else if (upperTypeId == 3 || upperTypeId == 4)
                            {
                                typeState = 2;
                            }
                            else
                            {
                                typeState = 1;
                            }
                        }
                        else if (prevTypeId == 3)
                        {
                            typeState = (upperTypeId != 4) ? 1 : 2;
                        }
                        else
                        {
                            if (upperTypeId == 3 || upperTypeId == 4)
                            {
                                typeState = 3;
                            }
                            else
                            {
                                typeState = 0;
                            }
                        }
                        break;
                    case 4:
                        if (prevTypeId == 3)
                        {
                            typeState = 0;
                        }
                        else if (prevTypeId == 4)
                        {
                            typeState = prevTypeState;
                        }
                        else
                        {
                            typeState = 1;
                        }
                        break;

                }

                Debug.Log($"[{i},{j}]: {wallTypeId}");

                var wtPos = new Vector2(posX, posY);
                Debug.Log($"pos: {posX},{posY}");
                if (wallTypeId != 0)
                {
                    GameObject goWallType = Instantiate(Resources.Load<GameObject>("wall/" + wallTypeId.ToString()), wtPos, Quaternion.identity);
                    goWallType.transform.localScale = new Vector3(0.5f, 0.5f);
                    //goWallType.transform.RotateAround(transform.position, transform.up, typeState * 90);
                    var rotateAngle = typeState * -90;
                Debug.Log($"rotateAngle: {rotateAngle}");
                    goWallType.transform.Rotate(0, 0, rotateAngle, Space.Self);
                }
                posX += 0.5f;

                prevTypeId = wallTypeId;
                prevTypeState = typeState;
                stateMap[i, j] = typeState;

            }
            posY -= 0.5f;

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
