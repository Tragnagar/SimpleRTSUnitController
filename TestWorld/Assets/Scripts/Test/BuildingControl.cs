using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    CommandCentre,
    FighterFactory,
    Shipyard
}

/// <summary>
/// Общий класс, описывающий управление всеми зданиями, доступными игроку.
/// </summary>
public class BuildingControl : UnitControl {

    // ====================================================================================
    // TODO:                                                                              
    // 1) Сделать префабы статичных объектов.                                             
    // 2) Настроить метод MouseControl() родительского объекта так, чтобы при клике мы-   
    //    ши создавалась целевая точка, но объект не двигался.                            
    // 3) Настроить создание подвижных объектов по щелчку мыши и их движение в заданную  
    //    точку.                                                                          
    // ====================================================================================

    [Header("Set in Inspector: Building Control")]
    [SerializeField] protected BuildingType buildingType;

    void Start () {
		
	}
	
	void Update () {
		
	}
}
