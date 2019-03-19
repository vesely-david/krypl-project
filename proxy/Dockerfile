FROM jwilder/nginx-proxy
RUN { \
      echo 'index index.html index.htm;'; \
      echo 'try_files $uri $uri/ /index.html;'; \
    } > /etc/nginx/conf.d/kryplproject.cz_location
RUN ln -s /etc/nginx/conf.d/kryplproject.cz_location /etc/nginx/conf.d/www.kryplproject.cz_location


    
    