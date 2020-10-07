///qnet_server_instance_create(x, y, obj)
if(global.is_host){
    var _id = instance_create(argument0, argument1, argument2);
    var arr = array_create(2);
    arr[0] = argument2;
    arr[1] = _id;
    qnet_server_queue_reliable_put_array_int("instance_create", arr);
    
    return _id;
}
