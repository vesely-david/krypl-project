FROM jwilder/nginx-proxy
RUN { \
      echo 'index index.html index.htm;'; \
      echo 'try_files $uri $uri/ /index.html;'; \
    } > /etc/nginx/conf.d/jankirchner.cz_location
RUN ln -s /etc/nginx/conf.d/jankirchner.cz_location /etc/nginx/conf.d/www.jankirchner.cz_location


    
    