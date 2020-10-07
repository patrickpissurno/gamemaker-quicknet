///qnet_encode_array(array)

var len = array_length_1d(argument0);
if(len == 0)
{
    return "";
}
else if(len == 1)
{
    return base64_encode(argument0);
}
else
{
    var result = base64_encode(argument0[0]);
    
    for(var i = 1; i < len; i += 1)
        result += ";" + base64_encode(argument0[i]);
        
    return result;    
}
