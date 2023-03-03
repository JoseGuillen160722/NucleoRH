function loginQr() {
    event.preventDefault();
    const html5QrCode = new Html5Qrcode("reader");

    $("#modalQR").modal("show");
    Html5Qrcode.getCameras().then(devices => {
        /**
         * devices would be an array of objects of type:
         * { id: "id", label: "label" }
         */
        console.log(devices) 
        if (devices && devices.length) {
            var cameraId = devices[0].id;
            // .. use this to start scanning.
            html5QrCode.start(
                { facingMode: "environment" },     // retreived in the previous step.
                {
                    fps: 1,    // sets the framerate to 10 frame per second
                    qrbox: 250  // sets only 250 X 250 region of viewfinder to
                    // scannable, rest shaded.
                },
                qrCodeMessage => {
                    // do something when code is read. For example:
                    console.log(`QR Code detected: ${qrCodeMessage}`);

                    //var jsonuser = JSON.parse(qrCodeMessage);
                    //$("#Input_Email").val(" ");
                    //$("#Input_Password").val(" ");

                    $("#Input_IdUsuario").val(qrCodeMessage);

                    $("#btnLogin").click();
                },
                errorMessage => {
                    // parse error, ideally ignore it. For example:
                    console.log(`QR Code no longer in front of camera.`);
                })
                .catch(err => {
                    // Start failed, handle it. For example,
                    console.log(`Unable to start scanning, error: ${err}`);
                });
        }
    }).catch(err => {
        // handle err
    });
}