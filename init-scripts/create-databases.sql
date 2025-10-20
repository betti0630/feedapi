CREATE DATABASE IF NOT EXISTS attrecto_aim;
CREATE DATABASE IF NOT EXISTS attrecto_feed;
GRANT ALL PRIVILEGES ON attrecto_aim.* TO 'myuser'@'%';
GRANT ALL PRIVILEGES ON attrecto_feed.* TO 'myuser'@'%';
FLUSH PRIVILEGES;