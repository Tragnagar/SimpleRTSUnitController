using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Общий класс, описывающий реакцию любого игрового объекта на наведение курсора и выделение.
/// </summary>
public class UnitControl : MonoBehaviour , IPointerClickHandler {

    // ====================================================================================
    // TODO:
    // 1) Настроить реакцию юнита на двойной щелчок мыши (с выделением всех видимых юни-
    //    тов данного типа).
    // ====================================================================================

    [Header("Set in Inspector")]
    public GameObject targetPointPrefab;            // Пустой объект, который будет служить путевой точкой
    public LayerMask mask;                          // Позволяет указать слой, который реагирует на клик
    
    [Header("Set Dynamically")]
    public bool selected = false;                   // Маркер выбора юнита
    public bool arrowOn = false;                    // Маркер того, что на юнит наведен курсор

    protected GameObject targetPoint;               // Объект-цель
    protected Light testLight;                      // Тестовая подсветка, для проверки выделения юнита

    protected Vector3 direction;                    // Направление на точку
    
    protected RaycastHit hit;                       // Объект попадания указателем мыши

    public float clicked = 0;
    public float clickTime = 0;
    public float clickDelay = 0.5f;

	void LateUpdate () {

        // Если указатель над юнитом...
        if (arrowOn)
        {
            // Если нажата ЛКМ...
            if (Input.GetMouseButtonDown(0))
            {
                selected = true;
            }
        }

        // Если указатель не над юнитом...
        if (!arrowOn)
        {
            // Если нажата ЛКМ...
            if (Input.GetMouseButtonDown(0))
            {
                selected = false;

                // ТЕСТ
                testLight.range = 0;
            }
        }
	}

    public void OnPointerClick(PointerEventData data)
    {
        int clickCount = data.clickCount;

        if (clickCount == 2)
        {
            Debug.Log(gameObject.name + " was Double Clicked!");
        }
    }

    public virtual void MouseControl() 
    {
        // Уничтожить предыдущий объект-цель
        Destroy(targetPoint);

        // Создать луч бесконечной длины
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Если луч пронзил указанный слой...
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            // Создать объект-цель
            targetPoint = Instantiate(targetPointPrefab) as GameObject;

            // Переместить его на позицию, указанную курсором мыши
            targetPoint.transform.position = hit.point;

            // Вычисление направления
            direction = targetPoint.transform.position - transform.position;
            direction = new Vector3(direction.x, 0, direction.z);
            direction.Normalize();
        }
    }

    /// <summary>
    /// Метод, выполняющий проверку того, что объект находится на экране.
    /// </summary>
    public bool CheckObjOnScreen()
    {
        // Локальная переменная с временной позицией в координатах области видимости камеры
        Vector3 tempPos = Camera.main.WorldToViewportPoint(transform.position);

        // Возвращаемая локальная переменная
        bool onScreen = false;

        // Проверка того, что объект находится в области видимости камеры
        if ((tempPos.x >= 0 && tempPos.x <= 1) && (tempPos.y >= 0 && tempPos.y <= 1)) onScreen = true;

        if ((tempPos.x < 0 || tempPos.x > 1) || (tempPos.y < 0 || tempPos.y > 1)) onScreen = false;

        return onScreen;
    }

    void OnMouseEnter()
    {
        arrowOn = true;

        // ТЕСТ
        testLight.range = 5;
    }

    void OnMouseExit()
    {
        arrowOn = false;

        // ТЕСТОВОЕ УСЛОВИЕ
        if (!selected) testLight.range = 0;
    }
}
