CREATE TABLE Teacher (
	id INT PRIMARY KEY,
    account_name NVarchar(100) NOT NULL,
    email VARCHAR(150) NOT NULL,
    account_type VARCHAR(10),
   	isValid BIT
);

CREATE TABLE ClassRoom (
	id INT PRIMARY KEY,
    year INT NOT NULL,
    semester INT NOT NULL,
    grade INT NOT NULL,
    class_id INT NOT NULL,
   	class_name NVarchar(50),
   	class_type NVarchar(50),
   	class_group NVarchar(50)
);

CREATE INDEX ClassRoomIndex 
ON ClassRoom (year, class_id);

CREATE TABLE Student (
    id INT PRIMARY KEY,
    year INT NOT NULL,
    semester INT NOT NULL,
    student_name NVarchar(50),
    seat VARCHAR(20),
    class_id INT
);

INSERT INTO ClassRoom (id, year, semester, grade, class_id, class_name)
VALUES
    (212, 108, 1, 1, 14343432, N'體育一真'),
    (523, 108, 1, 1, 57838582, N'美術一真'),
    (8742, 108, 1, 1, 39836729, N'管琴樂一真');

INSERT INTO Ｔeacher (id, account_name, email, account_type, isValid)
VALUES
    (0905556696, 'Hsinpa', 'hsinpa@gmail.com', 'teacher', 1),
    (339939823, 'FakeTeacher_1', 'faket_1@gmail.com', 'teacher', 1),
    (493825793, 'FakeTeacher_2', 'faket_2@gmail.com', 'teacher', 1),
    (32330021, 'FakeTeacher_3', 'faket_3@gmail.com', 'teacher', 0);

INSERT INTO Student (id, year, semester, student_name, seat, class_id)
VALUES
    (4897347, 108, 1, 'FakeSport_1', '1', 14343432),
    (1234123, 108, 1, 'FakeSport_2', '1', 14343432),
    (545234, 108, 1, 'FakeSport_3', '1', 14343432),
    (123412, 108, 1, 'FakeSport_4', '1', 14343432),
    (124125, 108, 1, 'FakeSport_5', '1', 14343432),
    (988894, 108, 1, 'FakeSport_6', '1', 14343432),
    (986532, 108, 1, 'FakeSport_7', '1', 14343432),
    (997422, 108, 1, 'FakeSport_8', '1', 14343432),
    (998764, 108, 1, 'FakeSport_9', '1', 14343432),

    (763462, 108, 1, 'FakeArt_1', '1', 57838582),
    (1237732, 108, 1, 'FakeArt_2', '1', 57838582),
    (9746822, 108, 1, 'FakeArt_3', '1', 57838582),
    (4939482, 108, 1, 'FakeArt_4', '1', 57838582),
    (1020394, 108, 1, 'FakeArt_5', '1', 57838582),
    (4828290, 108, 1, 'FakeArt_6', '1', 57838582),
    (5779403, 108, 1, 'FakeArt7', '1', 57838582),
    (684802, 108, 1, 'FakeArt_8', '1', 57838582),
    (7940856, 108, 1, 'FakeArt_9', '1', 57838582),

    (14741588, 108, 1, 'FakeMusic_1', '1', 39836729),
    (48579302, 108, 1, 'FakeMusic_2', '1', 39836729),
    (20148854, 108, 1, 'FakeMusic_3', '1', 39836729),
    (90884733, 108, 1, 'FakeMusic_4', '1', 39836729),
    (39293934, 108, 1, 'FakeMusic_5', '1', 39836729),
    (58574949, 108, 1, 'FakeMusic_6', '1', 39836729),
    (10399875, 108, 1, 'FakeMusic_7', '1', 39836729),
    (4458744, 108, 1, 'FakeMusic_8', '1', 39836729),
    (39294874, 108, 1, 'FakeMusic_9', '1', 39836729);
