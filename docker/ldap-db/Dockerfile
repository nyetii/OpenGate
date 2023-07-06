FROM osixia/openldap:1.5.0
RUN rm -rf /container/service/slapd/assets/config/bootstrap/ldif/custom
RUN mkdir -p /container/service/slapd/assets/config/bootstrap/ldif/custom
ADD bootstrap /container/service/slapd/assets/config/bootstrap