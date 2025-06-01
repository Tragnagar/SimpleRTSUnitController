using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс отвечает за выделение юнитов рамкой рисуемой при нажатии левой кнопки мыши.
/// Выделять в группу можно только подвижные юниты (космические корабли и пр.).
/// Здания выделять в группу нельзя.
/// </summary>
public class BoxSelectionControl : MonoBehaviour {

    public static BoxSelectionControl S;                        // Объект-одиночка

    public static List<GameObject> unitOnScreen;                // Список объектов на экране
    public static List<GameObject> unitSelected;                // Список выбранных объектов

    public static bool removed = false;                         // Маркер удаления объекта из списка

    [Header("Set in Inspector")]
    public GUISkin skin;                                        // Элемент GUISkin

    private Rect rect;                                          // Прямоугольник-рамка
    private bool draw;                                          // Маркер начала отрисовки рамки
    private Vector2 startPos;                                   // Начальная позиция указателя мыши
    private Vector2 endPos;                                     // Конечная позиция указателя мыши

	void Awake () {
        S = this;

        // Объявление списка всех юнитов
        unitOnScreen = new List<GameObject>();
        // Объявление списка выбранных юнитов
        unitSelected = new List<GameObject>();
	}

    void OnGUI()
    {
        // Присвоить скин
        GUI.skin = skin;
        // Установить уровень сортировки
        GUI.depth = 99;

        if (Input.GetMouseButtonDown(0))
        {
            // Если нажата ЛКМ, зафиксировать координаты мыши
            startPos = Input.mousePosition;
            // Начать отрисовку
            draw = true;

            // Очистить список выбранных юнитов
            unitSelected.Clear();
            //Debug.Log("List contains " + unitSelected.Count);
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Если ЛКМ отпущена, прекратить отрисовку
            draw = false;
        }

        // Если отрисовка начата...
        if (draw)
        {
            // Присваивать конечным координатам текущие координаты указателя мыши
            endPos = Input.mousePosition;

            // Если начальная и конечная позиции совпадают, выйти
            if (startPos == endPos) return;

            // Нарисовать рамку между конечной и начальной позициями указателя мыши
            rect = new Rect(Mathf.Min(endPos.x, startPos.x), 
                Screen.height - Mathf.Max(endPos.y, startPos.y), 
                Mathf.Max(endPos.x, startPos.x) - Mathf.Min(endPos.x, startPos.x), 
                Mathf.Max(endPos.y, startPos.y) - Mathf.Min(endPos.y, startPos.y));

            // Проверка, что экранные координаты объекта на экране попадают в нарисованную рамку
            foreach (GameObject go in unitOnScreen)
            {
                // Локальная переменная, в которую записываются экранные координаты объекта
                Vector2 tempPos = new Vector2(Camera.main.WorldToScreenPoint(go.transform.position).x, Screen.height - Camera.main.WorldToScreenPoint(go.transform.position).y);

                // Если координаты попадают, и в списке нет этого объекта, то...
                if (rect.Contains(tempPos) && !unitSelected.Contains(go))
                {
                    // Добавить объект в список выбранных
                    unitSelected.Add(go);
                }
                else if (!rect.Contains(tempPos) && unitSelected.Contains(go))
                {
                    // Иначе, если рамка не содержит координат, но объект в списке, то удалить его из списка
                    unitSelected.Remove(go);
                    // Маркер того, что объект удален из списка
                    removed = true;
                    Debug.Log(go.name + " removed from UnitSelected");
                }
            }

            // Отрисовать рамку на слое GUI
            GUI.Box(rect, "");
        }
    }
}
