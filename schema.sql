SET CLIENT_ENCODING TO 'UTF8';

DROP TABLE IF EXISTS "user" CASCADE;

CREATE TABLE "user" (
    id VARCHAR(36) PRIMARY KEY,
    avatar VARCHAR(255),
    intro VARCHAR(255),
    nick_name VARCHAR(100),
    username VARCHAR(100),
    email VARCHAR(100),
    student_id VARCHAR(20),
    password VARCHAR(100),
    status INTEGER,
    update_time BIGINT,
    create_time BIGINT,
    address VARCHAR(10),
    check_nick_name VARCHAR(100),
    check_intro VARCHAR(255),
    check_avatar VARCHAR(255),
    check_status INTEGER
);

DROP TABLE IF EXISTS product_type CASCADE;

CREATE TABLE product_type (
    id VARCHAR(36) PRIMARY KEY,
    type_code VARCHAR(10) UNIQUE,
    type_name VARCHAR(20),
    create_time BIGINT,
    update_time BIGINT
);

DROP TABLE IF EXISTS product_info CASCADE;

CREATE TABLE product_info (
    id VARCHAR(36) PRIMARY KEY,
    user_id VARCHAR(36),
    title VARCHAR(100),
    intro TEXT,
    image TEXT,
    price BIGINT,
    original_price BIGINT,
    type_code VARCHAR(10),
    type_name VARCHAR(20),
    post_type INTEGER,
    like_count INTEGER,
    adcode VARCHAR(10),
    province VARCHAR(10),
    city VARCHAR(10),
    district VARCHAR(10),
    status INTEGER,
    create_time BIGINT,
    update_time BIGINT,
    FOREIGN KEY (user_id) REFERENCES "user" (id),
    FOREIGN KEY (type_code) REFERENCES product_type (type_code)
);

DROP TABLE IF EXISTS chat_list CASCADE;

CREATE TABLE chat_list (
    id VARCHAR(36) PRIMARY KEY,
    product_id VARCHAR(36) REFERENCES product_info (id),
    product_image VARCHAR(255),
    from_user_id VARCHAR(36) NOT NULL,
    from_user_avatar VARCHAR(100),
    from_user_nick VARCHAR(100),
    to_user_id VARCHAR(36) NOT NULL,
    to_user_nick VARCHAR(100),
    to_user_avatar VARCHAR(100),
    create_time BIGINT,
    update_time BIGINT
);

DROP TABLE IF EXISTS chat_message CASCADE;

CREATE TABLE chat_message (
    id VARCHAR(36) PRIMARY KEY,
    chat_list_id VARCHAR(36) NOT NULL REFERENCES chat_list (id),
    from_user_id VARCHAR(36) NOT NULL,
    to_user_id VARCHAR(36) NOT NULL,
    from_user_nick VARCHAR(100),
    to_user_nick VARCHAR(100),
    content TEXT,
    send_time BIGINT,
    is_read INTEGER
);

DROP TABLE IF EXISTS comment CASCADE;

CREATE TABLE comment (
    id VARCHAR(36) PRIMARY KEY,
    product_id VARCHAR(36),
    parent_id VARCHAR(36) REFERENCES comment (id),
    pub_user_id VARCHAR(36),
    pub_nickname VARCHAR(100),
    parent_user_id VARCHAR(36),
    parent_user_nickname VARCHAR(255),
    content TEXT,
    create_time BIGINT
);

DROP TABLE IF EXISTS payment_order CASCADE;

CREATE TABLE payment_order (
    id VARCHAR(36) PRIMARY KEY,
    order_number VARCHAR(32),
    user_id VARCHAR(36),
    pay_price BIGINT,
    pay_type_id BIGINT,
    pay_type_name VARCHAR(200),
    order_status INTEGER,
    payment_pay_id VARCHAR(36),
    payment_status INTEGER,
    payment_type VARCHAR(20),
    process_status INTEGER,
    time_create TIMESTAMPTZ,
    time_update TIMESTAMPTZ,
    time_finish TIMESTAMPTZ,
    FOREIGN KEY (user_id) REFERENCES "user" (id),
    FOREIGN KEY (pay_type_id) REFERENCES payment_type (id)
);

DROP TABLE IF EXISTS payment_pay CASCADE;

CREATE TABLE payment_pay (
    id VARCHAR(36) PRIMARY KEY,
    user_id VARCHAR(36),
    order_id VARCHAR(36),
    payment_price BIGINT,
    payment_type VARCHAR(30),
    payment_result_data TEXT,
    payment_time_start VARCHAR(50),
    payment_time_expire VARCHAR(50),
    payment_status INTEGER DEFAULT 0,
    process_status INTEGER DEFAULT 0,
    time_create TIMESTAMPTZ,
    time_update TIMESTAMPTZ,
    time_finish TIMESTAMPTZ,
    FOREIGN KEY (user_id) REFERENCES "user" (id),
    FOREIGN KEY (order_id) REFERENCES payment_order (id)
);

DROP TABLE IF EXISTS payment_type CASCADE;

CREATE TABLE payment_type (
    id BIGINT PRIMARY KEY,
    type_name VARCHAR(200),
    wx_pay NUMERIC(4, 2),
    ali_pay NUMERIC(4, 2)
);

DROP TABLE IF EXISTS product_collect CASCADE;

CREATE TABLE product_collect (
    id VARCHAR(36) PRIMARY KEY,
    user_id VARCHAR(36),
    product_id VARCHAR(36),
    create_time BIGINT,
    update_time BIGINT,
    FOREIGN KEY (user_id) REFERENCES "user" (id),
    FOREIGN KEY (product_id) REFERENCES product_info (id)
);

DROP TABLE IF EXISTS product_order CASCADE;

CREATE TABLE product_order (
    id VARCHAR(36) PRIMARY KEY,
    order_number VARCHAR(32),
    product_user_id VARCHAR(36),
    product_id VARCHAR(36),
    user_id VARCHAR(36),
    product_title VARCHAR(100),
    product_img TEXT,
    product_price BIGINT,
    product_type VARCHAR(30),
    product_type_name VARCHAR(100),
    product_sell_price BIGINT,
    product_num INTEGER,
    product_post BIGINT,
    product_post_status INTEGER,
    product_money BIGINT,
    product_info TEXT,
    buy_money_all BIGINT,
    buy_money BIGINT,
    buy_info VARCHAR(100),
    post_mode VARCHAR(40),
    post_self_code VARCHAR(6),
    post_username VARCHAR(100),
    post_phone VARCHAR(100),
    post_address VARCHAR(200),
    ship_username VARCHAR(100),
    ship_phone VARCHAR(100),
    ship_address VARCHAR(200),
    ship_num VARCHAR(100),
    ship_company VARCHAR(100),
    ship_time TIMESTAMPTZ,
    pay_status INTEGER,
    pay_order_id VARCHAR(36),
    deal_status INTEGER DEFAULT 0,
    eva_score INTEGER,
    eva_content TEXT,
    create_time TIMESTAMPTZ,
    update_time TIMESTAMPTZ,
    FOREIGN KEY (product_user_id) REFERENCES "user" (id),
    FOREIGN KEY (product_id) REFERENCES product_info (id),
    FOREIGN KEY (user_id) REFERENCES "user" (id),
    FOREIGN KEY (pay_order_id) REFERENCES payment_order (id)
);

DROP TABLE IF EXISTS product_voucher CASCADE;

CREATE TABLE product_voucher (
    id VARCHAR(36) PRIMARY KEY,
    product_id VARCHAR(36) NOT NULL REFERENCES product_info (id),
    title VARCHAR(255),
    voucher_value BIGINT,
    stock INTEGER,
    begin_time TIMESTAMPTZ,
    end_time TIMESTAMPTZ,
    voucher_status INTEGER,
    create_time BIGINT,
    update_time BIGINT
);

DROP TABLE IF EXISTS system_role CASCADE;

CREATE TABLE system_role (
    id VARCHAR(36) PRIMARY KEY,
    role_code VARCHAR(20),
    role_name VARCHAR(20),
    create_time BIGINT,
    update_time BIGINT
);

DROP TABLE IF EXISTS "system_user" CASCADE;

CREATE TABLE "system_user" (
    id VARCHAR(36) PRIMARY KEY,
    username VARCHAR(100),
    password VARCHAR(100),
    name VARCHAR(20),
    role_id VARCHAR(36),
    role_code VARCHAR(20),
    role_name VARCHAR(20),
    create_time BIGINT,
    update_time BIGINT,
    FOREIGN KEY (role_id) REFERENCES system_role (id)
);

DROP TABLE IF EXISTS user_address CASCADE;

CREATE TABLE user_address (
    id VARCHAR(36) PRIMARY KEY,
    user_id VARCHAR(36) NOT NULL REFERENCES "user" (id),
    name VARCHAR(10),
    phone VARCHAR(20),
    address VARCHAR(255),
    create_time BIGINT,
    update_time BIGINT
);

DROP TABLE IF EXISTS voucher_order CASCADE;

CREATE TABLE voucher_order (
    id VARCHAR(36) PRIMARY KEY,
    user_id VARCHAR(36),
    product_id VARCHAR(36),
    voucher_id VARCHAR(36),
    voucher_value BIGINT,
    status INTEGER,
    create_time BIGINT,
    update_time BIGINT,
    FOREIGN KEY (user_id) REFERENCES "user" (id),
    FOREIGN KEY (product_id) REFERENCES product_info (id),
    FOREIGN KEY (voucher_id) REFERENCES product_voucher (id)
);