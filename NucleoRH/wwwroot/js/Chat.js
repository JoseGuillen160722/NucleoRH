const connection = new signalR.HubConnectionBuilder()
    .withUrl("/ChatHub").build();

var urlParams = new URLSearchParams(window.location.search);
const group = urlParams.get('group') || "Chat_Home";

connection.on("ReceiveMessage", (user, message) => {

    const toast = swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    });

    toast({
        type: 'warning',
        title: message
    })

});

connection.start().then(() => {
    // Este bloque de código se ejecuta cuando se establece la conexión con el servidor
    connection.invoke("AddToGroup", group).catch(err => console.error(err.toString()));
}).catch(err => {
    console.error(err.toString());
});
