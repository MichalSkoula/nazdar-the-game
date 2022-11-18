hugo --gc --minify
rclone sync public/ iowa:/var/www/nazdarthegame.com
