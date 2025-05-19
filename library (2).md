## Задание: Создание консольного приложения для системы управления библиотекой

### Описание задачи  
Разработайте **консольное приложение** на базе **.NET 9** для управления библиотекой с использованием **Dapper** и **PostgreSQL**. Приложение должно обеспечивать полный цикл работы с данными о книгах, читателях и заимствованиях, включая проверки наличия копий, автоматический расчёт штрафов за просрочку возврата и валидацию пользовательского ввода.

### Требования

#### 1. База данных  
Создайте базу данных PostgreSQL с тремя основными таблицами:

1. **`Books` (Книги):**  
   - `BookId` (int, PK) — уникальный идентификатор книги.  
   - `Title` (varchar(200)) — название книги.  
   - `Genre` (varchar(100)) — жанр книги.  
   - `PublicationYear` (int) — год издания.  
   - `TotalCopies` (int) — общее количество экземпляров книги в библиотеке.  
   - `AvailableCopies` (int) — количество экземпляров, доступных для заимствования.  

2. **`Members` (Читатели):**  
   - `MemberId` (int, PK) — уникальный идентификатор читателя.  
   - `FullName` (varchar(150)) — полное имя читателя.  
   - `Phone` (varchar(20)) — номер телефона.  
   - `Email` (varchar(150)) — адрес электронной почты.  
   - `MembershipDate` (date) — дата регистрации в библиотеке.  

3. **`Borrowings` (Заимствования):**  
   - `BorrowingId` (int, PK) — уникальный идентификатор записи заимствования.  
   - `BookId` (int, FK -> Books.BookId) — идентификатор книги, которую заимствовали.  
   - `MemberId` (int, FK -> Members.MemberId) — идентификатор читателя.  
   - `BorrowDate` (date) — дата выдачи книги.  
   - `DueDate` (date) — дата, до которой книгу необходимо вернуть.  
   - `ReturnDate` (date, nullable) — дата фактического возврата книги.  
   - `Fine` (decimal(10,2)) — штраф за просрочку возврата, который рассчитывается автоматически, если `ReturnDate` превышает `DueDate`.  

#### 2. Функциональные требования

**Управление книгами (`Books`):**  
- **Добавление книги:** Ввод данных новой книги и сохранение записи в базе данных.  
- **Просмотр списка книг:** Вывод всех книг в консоль.  
- **Детальный просмотр:** Получение полной информации о конкретной книге по её идентификатору.  
- **Редактирование:** Обновление данных книги.  
- **Удаление:** Удаление записи о книге, при условии, что книга не находится в активном заимствовании.

**Управление читателями (`Members`):**  
- **Регистрация:** Добавление нового читателя с вводом необходимых персональных данных.  
- **Просмотр списка читателей:** Вывод списка всех зарегистрированных читателей в консоли.  
- **Детальный просмотр:** Получение информации о конкретном читателе по его идентификатору.  
- **Редактирование:** Обновление информации о читателе.  
- **Удаление:** Удаление записи о читателе, если у него отсутствуют активные заимствования.

**Управление заимствованиями (`Borrowings`):**  
- **Оформление заимствования:**  
  - Проверка наличия доступных копий книги (значение `AvailableCopies` > 0).  
  - Создание записи заимствования с указанием дат выдачи (`BorrowDate`) и возврата (`DueDate`).  
  - Автоматическое уменьшение `AvailableCopies` у выбранной книги.  
- **Просмотр заимствований:** Вывод списка всех записей заимствований в консоль.  
- **История заимствований:** Возможность просмотра полной истории заимствований для конкретного читателя по его идентификатору.  
- **Обработка возврата:**  
  - Ввод фактической даты возврата книги.  
  - Автоматический расчёт штрафа, если книга возвращена с опозданием (на основе разницы между `ReturnDate` и `DueDate`).  
  - Обновление записи заимствования, установка `ReturnDate` и `Fine`.  
  - Автоматическое увеличение `AvailableCopies` у книги.

#### 3. Требования к реализации

- **Используемые технологии:**  
  - **.NET 9** — для создания консольного приложения.  
  - **Dapper** — для легковесного доступа к базе данных.  
  - **Npgsql** — для подключения к PostgreSQL.

#### 4. Queries

1. **Самая популярная книга (с наибольшим количеством заимствований)** – `QuerySingleOrDefault`  
2. **Самый активный читатель (взял больше всех книг)** – `QuerySingleOrDefault`  
3. **Количество всех заимствованных книг** – `ExecuteScalar<int>`  
4. **Средний штраф за просрочку** – `ExecuteScalar<decimal>`  
5. **Список книг, которые сейчас у читателей (не возвращены)** – `Query`  
6. **Книги, у которых нет доступных экземпляров** – `Query`  
7. **Число книг, которые ни разу не брали** – `ExecuteScalar<int>`  
8. **Количество читателей, у которых есть хотя бы одно заимствование** – `ExecuteScalar<int>`  
9. **Самый популярный жанр книг** – `QuerySingleOrDefault`  
10. **Первый читатель, у которого есть просроченные возвраты** – `QueryFirstOrDefault`  
11. **Топ-5 читателей по количеству взятых книг** – `Query`  
12. **Книги, которые заимствовались больше 5 раз** – `Query`  
13. **Общая сумма всех штрафов** – `QuerySingleOrDefault`  
14. **Число книг, которые были возвращены с просрочкой** – `ExecuteScalar<int>`  
15. **Список читателей, которые заплатили штраф** – `Query`  

#### 5. Data
```sql
-- =========================================
-- INSERT STATEMENTS FOR TABLE: Books
-- =========================================
INSERT INTO Books (Title, Genre, PublicationYear, TotalCopies, AvailableCopies)
VALUES ('Война и мир', 'Роман', 1869, 10, 10);

INSERT INTO Books (Title, Genre, PublicationYear, TotalCopies, AvailableCopies)
VALUES ('Преступление и наказание', 'Роман', 1866, 8, 8);

INSERT INTO Books (Title, Genre, PublicationYear, TotalCopies, AvailableCopies)
VALUES ('Мастер и Маргарита', 'Фантастика', 1967, 12, 12);

INSERT INTO Books (Title, Genre, PublicationYear, TotalCopies, AvailableCopies)
VALUES ('Анна Каренина', 'Роман', 1877, 9, 9);

INSERT INTO Books (Title, Genre, PublicationYear, TotalCopies, AvailableCopies)
VALUES ('Евгений Онегин', 'Поэма', 1833, 7, 7);

INSERT INTO Books (Title, Genre, PublicationYear, TotalCopies, AvailableCopies)
VALUES ('Отцы и дети', 'Роман', 1862, 6, 6);

INSERT INTO Books (Title, Genre, PublicationYear, TotalCopies, AvailableCopies)
VALUES ('Обломов', 'Роман', 1859, 5, 5);

INSERT INTO Books (Title, Genre, PublicationYear, TotalCopies, AvailableCopies)
VALUES ('Идиот', 'Роман', 1869, 8, 8);

INSERT INTO Books (Title, Genre, PublicationYear, TotalCopies, AvailableCopies)
VALUES ('Ревизор', 'Комедия', 1836, 4, 4);

INSERT INTO Books (Title, Genre, PublicationYear, TotalCopies, AvailableCopies)
VALUES ('Тихий Дон', 'Роман', 1928, 10, 10);

-- =========================================
-- INSERT STATEMENTS FOR TABLE: Members
-- =========================================
INSERT INTO Members (FullName, Phone, Email, MembershipDate)
VALUES ('Иван Иванов', '1234567890', 'ivanov@example.com', '2023-01-15');

INSERT INTO Members (FullName, Phone, Email, MembershipDate)
VALUES ('Петр Петров', '0987654321', 'petrov@example.com', '2023-02-20');

INSERT INTO Members (FullName, Phone, Email, MembershipDate)
VALUES ('Сергей Сергеев', '5551234567', 'sergeev@example.com', '2023-03-05');

INSERT INTO Members (FullName, Phone, Email, MembershipDate)
VALUES ('Алексей Смирнов', '7778889990', 'smirnov@example.com', '2023-04-10');

INSERT INTO Members (FullName, Phone, Email, MembershipDate)
VALUES ('Мария Козлова', '2223334445', 'kozlova@example.com', '2023-05-15');

INSERT INTO Members (FullName, Phone, Email, MembershipDate)
VALUES ('Екатерина Соколова', '1112223334', 'sokolova@example.com', '2023-06-01');

INSERT INTO Members (FullName, Phone, Email, MembershipDate)
VALUES ('Дмитрий Федоров', '9998887776', 'fedorov@example.com', '2023-07-20');

INSERT INTO Members (FullName, Phone, Email, MembershipDate)
VALUES ('Ольга Николаева', '4445556667', 'nikolaeva@example.com', '2023-08-05');

INSERT INTO Members (FullName, Phone, Email, MembershipDate)
VALUES ('Анастасия Лебедева', '3334445556', 'lebedeva@example.com', '2023-09-10');

INSERT INTO Members (FullName, Phone, Email, MembershipDate)
VALUES ('Виктория Морозова', '8887776665', 'morozova@example.com', '2023-10-01');

-- =========================================
-- INSERT STATEMENTS FOR TABLE: Borrowings
-- =========================================
INSERT INTO Borrowings (BookId, MemberId, BorrowDate, DueDate, ReturnDate, Fine)
VALUES (1, 1, '2023-04-01', '2023-04-08', NULL, 0.00);

INSERT INTO Borrowings (BookId, MemberId, BorrowDate, DueDate, ReturnDate, Fine)
VALUES (2, 2, '2023-04-03', '2023-04-10', '2023-04-11', 5.00);

INSERT INTO Borrowings (BookId, MemberId, BorrowDate, DueDate, ReturnDate, Fine)
VALUES (3, 3, '2023-04-05', '2023-04-12', NULL, 0.00);

INSERT INTO Borrowings (BookId, MemberId, BorrowDate, DueDate, ReturnDate, Fine)
VALUES (4, 4, '2023-04-07', '2023-04-14', '2023-04-15', 3.00);

INSERT INTO Borrowings (BookId, MemberId, BorrowDate, DueDate, ReturnDate, Fine)
VALUES (5, 5, '2023-04-09', '2023-04-16', NULL, 0.00);

INSERT INTO Borrowings (BookId, MemberId, BorrowDate, DueDate, ReturnDate, Fine)
VALUES (6, 6, '2023-04-11', '2023-04-18', '2023-04-20', 4.00);

INSERT INTO Borrowings (BookId, MemberId, BorrowDate, DueDate, ReturnDate, Fine)
VALUES (7, 7, '2023-04-13', '2023-04-20', NULL, 0.00);

INSERT INTO Borrowings (BookId, MemberId, BorrowDate, DueDate, ReturnDate, Fine)
VALUES (8, 8, '2023-04-15', '2023-04-22', '2023-04-23', 2.00);

INSERT INTO Borrowings (BookId, MemberId, BorrowDate, DueDate, ReturnDate, Fine)
VALUES (9, 9, '2023-04-17', '2023-04-24', NULL, 0.00);

INSERT INTO Borrowings (BookId, MemberId, BorrowDate, DueDate, ReturnDate, Fine)
VALUES (10, 10, '2023-04-19', '2023-04-26', '2023-04-27', 1.00);
```

#### 4. Queries

1. **Самая популярная книга (с наибольшим количеством заимствований)** – `QuerySingleOrDefault`  
2. **Самый активный читатель (взял больше всех книг)** – `QuerySingleOrDefault`  
3. **Количество всех заимствованных книг** – `ExecuteScalar<int>`  
4. **Средний штраф за просрочку** – `ExecuteScalar<decimal>`  
5. **Список книг, которые сейчас у читателей (не возвращены)** – `Query`  
6. **Книги, у которых нет доступных экземпляров** – `Query`  
7. **Число книг, которые ни разу не брали** – `ExecuteScalar<int>`  
8. **Количество читателей, у которых есть хотя бы одно заимствование** – `ExecuteScalar<int>`  
9. **Самый популярный жанр книг** – `QuerySingleOrDefault`  
10. **Первый читатель, у которого есть просроченные возвраты** – `QueryFirstOrDefault`  
11. **Топ-5 читателей по количеству взятых книг** – `Query`  
12. **Книги, которые заимствовались больше 5 раз** – `Query`  
13. **Общая сумма всех штрафов** – `QuerySingleOrDefault`  
14. **Число книг, которые были возвращены с просрочкой** – `ExecuteScalar<int>`  
15. **Список читателей, которые заплатили штраф** – `Query`