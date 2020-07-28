CREATE TABLE Teacher (
	id INT PRIMARY KEY AUTO_INCREMENT,
    account_id INT NOT NULL,
    account_name INT NOT NULL,
    email INT NOT NULL,
    account_type INT NOT NULL,
   	isValid BIT
);

CREATE TABLE ClassRoom (
	id INT PRIMARY KEY,
    year INT NOT NULL,
    semester INT NOT NULL,
    grade INT NOT NULL,
    class_id INT NOT NULL,
   	name VARCHAR(50),
   	class_type VARCHAR(50),
   	class_group VARCHAR(50)
);

CREATE TABLE Student (
    id INT PRIMARY KEY AUTO_INCREMENT,
    year INT NOT NULL,
    semester INT NOT NULL,
    name VARCHAR(50),
    seat VARCHAR(20),
    class_id INT
    -- FOREIGN KEY (store_id) REFERENCES sales.stores (store_id)
);