-- Database CampusMarket

CREATE TABLE user (
    id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(64) NOT NULL,
    password VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    nickname VARCHAR(64) NOT NULL,
    bio TEXT,
    avatar UUID,
    admin BOOLEAN NOT NULL DEFAULT FALSE,
    status ENUM('unverified', 'verified', 'banned') NOT NULL DEFAULT 'unverified',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    last_login_at TIMESTAMP,
    UNIQUE (username),
    UNIQUE (email)
);

CREATE TABLE post (
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    title VARCHAR(255) NOT NULL,
    description TEXT,
);
