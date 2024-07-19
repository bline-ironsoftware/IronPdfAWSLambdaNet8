FROM public.ecr.aws/lambda/dotnet:8

WORKDIR /var/task

# install necessary packages
RUN dnf update -y
RUN dnf install -y gcc-c++ pango.x86_64 libXcomposite.x86_64 libXcursor.x86_64 dbus-glib-devel
RUN dnf install -y libXdamage.x86_64 libXi.x86_64 libXtst.x86_64 cups-libs.x86_64 libXScrnSaver.x86_64 
RUN dnf install -y libXrandr.x86_64 alsa-lib.x86_64 atk.x86_64 gtk3.x86_64 ipa-gothic-fonts xorg-x11-fonts-100dpi
RUN dnf install -y xorg-x11-fonts-75dpi xorg-x11-utils xorg-x11-fonts-cyrillic xorg-x11-fonts-Type1 xorg-x11-fonts-misc
RUN dnf install -y mesa-libgbm.x86_64
RUN dnf install -y nss-3.90.0-3.amzn2023.0.4.x86_64

COPY "bin/Release/lambda-publish"  .
