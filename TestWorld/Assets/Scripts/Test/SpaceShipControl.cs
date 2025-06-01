using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Общий класс, описывающий базовое управление всеми космическими кораблями,
/// доступными игроку.
/// </summary>
public class SpaceShipControl : UnitControl {

    // ====================================================================================
    // TODO:                                                                              
    // 1) Настроить столкновения объкектов при движении, а также их взаимодействие.       
    // ====================================================================================

    [Header("Set in Inspector: Space Ship")]
    public float speedRotation;                     // Скорость поворота
    public float speedWalking;                      // Скорость перемещения

    [Header("Set Dynamically: Space Ship")]
    public bool moving = false;                     // Маркер начала движения
    public bool onPlace = true;                     // Маркер достижения точки назначения

    void Start () {
        testLight = GetComponent<Light>();      // Тестовая строка
	}
	
	void Update () {

        // Если объект на экране и он ещё не находится в списке объектов на экране, то...
        if (CheckObjOnScreen() && !BoxSelectionControl.unitOnScreen.Contains(gameObject))
        {
            // ...добавить его в список
            BoxSelectionControl.unitOnScreen.Add(gameObject);

            TestListContainer(BoxSelectionControl.unitOnScreen);        // Тестовая строка
        }

        // Если объект не на экране и содержится в списке экранных объектов, то...
        if (!CheckObjOnScreen() && BoxSelectionControl.unitOnScreen.Contains(gameObject))
        {
            // ...удалить его из списка
            BoxSelectionControl.unitOnScreen.Remove(gameObject);

            TestListContainer(BoxSelectionControl.unitOnScreen);        // Тестовая строка
        }

        // Если объект в списке выбранных, то перевести selected в true
        if (BoxSelectionControl.unitSelected.Contains(gameObject))
        {
            selected = true;
            testLight.range = 5;        // Тестовая строка
        }
        else if (!BoxSelectionControl.unitSelected.Contains(gameObject) && BoxSelectionControl.removed)
        {
            selected = false;
            testLight.range = 0;        // Тестовая строка

            // Маркер удаления снова перевести в false
            BoxSelectionControl.removed = false;
        }

        // Если объект выбран,...
        if (selected)
        {
            // ...то если нажата ПКМ,...
            if (Input.GetMouseButtonDown(1))
            {
                // ...вызвать метод MouseControl()
                MouseControl();
            }
        }

        // Если маркер движения true, а маркер нахождения в точке false,...
        if (moving && !onPlace)
        {
            // ...вызвать метод Move()
            Move();
        }
	}

    // Метод, описывающий движение объекта
    // Добавить определение столкновений, чтобы объект мог огибать препятствия
    public virtual void Move()
    {
        // Если выполнено, то...
        if (moving && !onPlace)
        {
            // Поворот
            Quaternion look = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * speedRotation);

            // Передвинуть объект (пока тест, потом переделать)
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(targetPoint.transform.position.x, transform.position.y, targetPoint.transform.position.z),
                speedWalking * Time.deltaTime);

            // Если объект достиг точки, то остановить его
            if (transform.position.x == targetPoint.transform.position.x &&
                transform.position.z == targetPoint.transform.position.z)
            {
                // Прекратить движение
                moving = false;
                // Объект на месте
                onPlace = true;

                // Уничтожить целевую точку
                Destroy(targetPoint);
            }
        }
    }

    /// <summary>
    /// Тестовый метод, выводящий имена всех объектов, содержащихся в списке, поданном на вход.
    /// </summary>
    public void TestListContainer(List<GameObject> list)
    {
        Debug.Log("This list contains:");

        // Для каждого элемента в списке вывести имя
        for(int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i].name);
        }
    }

    // Переопределение базового метода MouseControl()
    public override void MouseControl()
    {
        // Вызвать базовый метод
        base.MouseControl();

        // Двигаться
        moving = true;
        // Не на месте
        onPlace = false;
    }

    //public void OnCollisionEnter(Collision coll)
    //{
    //    GameObject collGO = coll.gameObject;

    //    Debug.Log(collGO.name + " have collision with " + gameObject.name);
    //}


}