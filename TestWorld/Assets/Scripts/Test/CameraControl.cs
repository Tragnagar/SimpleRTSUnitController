using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс описывает управление камерой.
/// </summary>
public class CameraControl : MonoBehaviour {
    // =================================================================================================
    // TODO:
    // 1) Написать метод BoundsCheck(), не позволяющий выйти камере за пределы игровой области.
    // 2) Настроить вертикальное перемещение (вверх/вниз) в режиме свободной камеры
    // 3) Настроить минимальное/максимальное перемещение камеры к объектам
    // =================================================================================================

    [Header("Set in Inspector")]
    public float cameraSpeed = 10f;             // Скорость камеры
    public float mouseWheelSpeed = 10f;         // Скорость приближения камеры
    public float mouseRotationSpeed = 10f;      // Скорость вращения камеры
    public float mouseSens = 0.1f;              // Чувствительность мыши

    public bool freeCam = false;                // Маркер перемещения камеры

    [Header("Set Dynamically")]
    public float screenWidth;                   // Ширина экрана
    public float screenHeight;                  // Высота экрана
    public bool inverted = false;               // Маркер инверсии мыши
    public Vector3 mousePos;                    // После теста убрать из списка переменных в локальные Update
    public float mouseWheel;                    // После теста убрать из списка переменных в локальные Update

    private Vector3 originAngles;               // Оригинальное значение углов поворота камеры (сделать private)

    void Awake () {
        // Получить текущую высоту экрана
        screenHeight = Display.main.renderingHeight;
        // Получить текущую ширину экрана
        screenWidth = Display.main.renderingWidth;

        // Сохранить начальную ориентацию камеры
        originAngles = transform.eulerAngles; 
	}
    
    void Update () {
        // Проверка того, что камера в залоченном режиме
        if (!freeCam)
        {
            // Получить экранные координаты мыши
            mousePos = Input.mousePosition;

            // Получить прокрутку колесика мыши
            mouseWheel = Input.GetAxis("Mouse ScrollWheel");

            // Проверить, пересек ли указатель мыши правую/левую границы экрана
            if (mousePos.x >= screenWidth)
            {
                // Сдвинуть камеру по оси Х вправо
                transform.position += transform.right * Time.deltaTime * cameraSpeed;
            }
            else if (mousePos.x <= 0)
            {
                // Сдвинуть камеру по оси Х влево
                transform.position -= transform.right * Time.deltaTime * cameraSpeed;
            }

            // Проверить, пересек ли указатель мыши верхнюю/нижнюю границы экрана
            if (mousePos.y >= screenHeight)
            {
                // Сформировать новый вектор с компонентами вектора transform.forward
                Vector3 direction = new Vector3(transform.forward.x, 0, transform.forward.z);
                // Получить направление вектора
                direction.Normalize();
                // Сдвинуть камеру вперед
                transform.position += direction * Time.deltaTime * cameraSpeed;
            }
            else if (mousePos.y <= 0)
            {
                // Сформировать новый вектор с компонентами вектора transform.forward
                Vector3 direction = new Vector3(transform.forward.x, 0, transform.forward.z);
                // Получить направление вектора
                direction.Normalize();
                // Сдвинуть камеру назад
                transform.position -= direction * Time.deltaTime * cameraSpeed;
            }

            // Если нажата СКМ, совершить поворот камеры вокруг оси Y в плоскости XoZ
            if (Input.GetMouseButton(2))
            {
                // Получить перемещение мыши по горизонтальной оси
                float xMouse = Input.GetAxis("Mouse X");

                // Локальная переменная angles
                Vector3 angles = transform.eulerAngles;
                // Изменить angles в соответствии с перемещением мыши (с учетом чувствительности)
                angles.y += xMouse * mouseRotationSpeed * mouseSens;
                // Присвоить углам камеры новые значение
                transform.eulerAngles = angles;
            }

            // Проверка условия, что колесико мыши вращается
            // (сделать ограничение на приближение/удаление камеры к/от объекту)
            if (mouseWheel != 0)
            {
                // Изменить позицию камеры относительно оси Z
                transform.position += transform.forward * mouseWheel * mouseWheelSpeed;
            }
        }

        // Проверка нажатия клавиши LeftAlt (включение свободной камеры и перемещения)
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            freeCam = true;

            // Получить перемещение мыши по осям X и Y
            float xMouse = Input.GetAxis("Mouse X");
            float yMouse = Input.GetAxis("Mouse Y");

            float xAxis = Input.GetAxis("Horizontal");
            float yAxis = Input.GetAxis("Vertical");

            // Локальная переменная angles, в которую записываются все изменения углов
            Vector3 angles = transform.eulerAngles;
            // Изменить компоненту Х в соответствии с перемещением мыши
            angles.x += yMouse * mouseRotationSpeed * mouseSens;
            // Проверить, чтобы углы поворота вокруг оси Х не выходили за установленные пределы
            if (angles.x > 80 && angles.x < 90)
            {
                // Установить значение угла равным 80 (камера смотрит вниз)
                angles.x = 80;
                // Обнулить показатель движения мыши, чтобы не изменялся угол (похоже на костыль, потом обдумать)
                yMouse = 0;
            } 
            else if (angles.x < 280 && angles.x > 270)
            {
                // Установить значение угла 280 (камера смотрит вверх)
                angles.x = 280;
                // Обнулить показатель движения мыши, чтобы не изменялся угол
                yMouse = 0;
            }
            // Изменить компоненту Y в соответствии с перемещением мыши
            angles.y += xMouse * mouseRotationSpeed * mouseSens;

            // Изменить положение камеры в зависимости от нажатых клавиш
            transform.position += (transform.forward * yAxis + transform.right * xAxis) * 
                cameraSpeed * Time.deltaTime;

            // Присвоить углам камеры новые значения
            transform.eulerAngles = angles;
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt)) freeCam = false;

        // При нажатии клавиши Х камера занимает начальную ориентацию
        if (Input.GetKeyDown(KeyCode.X)) transform.eulerAngles = originAngles;
    }

    // Нарисовать границы экрана в окне сцены
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector3 boundsSize = new Vector3(screenWidth, screenHeight, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundsSize);
    }

    // Метод, не позволяющий камере выйти за пределы игровой области
    private void BoundsCheck()
    {

    }
}
