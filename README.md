# Geometry API

Web API для управления геометрическими фигурами.

## Поддерживаемые фигуры

- **Окружность** (Circle) - координаты центра (x, y) и диаметр
- **Прямоугольник** (Rectangle) - координаты верхней левой и нижней правой точки
- **Треугольник** (Triangle) - координаты трех точек

## Особенности реализации

- Для хранения фигур используется SQLite, сумма площадей пишется в In-Memory Cache и обновляется при добавлении фигуры.

## Запуск

```
# Восстановить зависимости
dotnet restore

# Запустить приложение
dotnet run
```

Запускается на `http://localhost:5088`

Swagger: `http://localhost:5088/swagger`

## Эндпоинты

### 1. Создать фигуру
```http
POST /api/figures
Content-Type: application/json
```

**Примеры:**

**Окружность:**
```json
{
  "type": "circle",
  "center": {
    "x": 1,
    "y": 0
  },
  "diameter": 6
}
```

**Прямоугольник:**
```json
{
  "type": "rectangle",
  "leftUpper": {
    "x": 0,
    "y": 5
  },
  "rightBottom": {
    "x": 3,
    "y": 0
  }
}
```

**Треугольник:**
```json
{
  "type": "triangle",
  "leftBottom": {
    "x": 0,
    "y": 0
  },
  "rightBottom": {
    "x": 4,
    "y": 0
  },
  "upper": {
    "x": 2,
    "y": 3
  }
}
```

**Ответ:** `201 Created`
```json
{
  "id": 1,
  "type": "Circle",
  "square": 28.25...,
  "center": {
    "x": 1,
    "y": 0
  },
  "diameter": 6
}
```

**Ответ с ошибкой:** `400 Bad Request`
```json
{
  "error": "center обязателен для типа circle"
}
```

### 2. Получить фигуру по id
```http
GET /api/figures/{id}
```

**Ответ:** `200 OK`
```json
{
  "id": 1,
  "type": "Circle",
  "square": 28.25...,
  "center": {
    "x": 1,
    "y": 0
  },
  "diameter": 6
}
```

### 3. Получить сумму площадей всех фигур
```http
GET /api/figures/sum-squares
```

**Ответ:** `200 OK`
```json
174.12..
```
