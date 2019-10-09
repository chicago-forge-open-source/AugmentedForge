FROM balenalib/rpi-raspbian:latest

RUN apt-get -q update && \
	apt-get -qy install \
	    omxplayer \ 
        python \
        python-pip \
        python-setuptools \
        python-dev \
        gcc \
        make  

RUN pip install AWSIoTPythonSDK

COPY . /app/
RUN chmod +x /app/light_control/light_it_up.py

WORKDIR /app

ENTRYPOINT ["/app/light_control/light_it_up.py"]