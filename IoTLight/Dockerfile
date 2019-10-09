FROM balenalib/rpi-raspbian:latest

RUN apt-get -q update && \
	apt-get -qy install \
	    omxplayer \ 
        python \
        python-gi \
        python-pip \
        python-setuptools \
        python-dev \
        gcc \
        make  
RUN apt-get -q update && \
	apt-get -qy install \
        python-gst-1.0 \
        gstreamer1.0-plugins-good \
        gstreamer1.0-plugins-ugly \
        gstreamer1.0-tools

RUN pip install AWSIoTPythonSDK && \
    pip install playsound

COPY . /app/
RUN chmod +x /app/light_control/light_it_up.py

WORKDIR /app

ENTRYPOINT ["/app/light_control/light_it_up.py"]