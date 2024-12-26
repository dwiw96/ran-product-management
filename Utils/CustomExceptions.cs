namespace ran_product_management_net.Utils;

public class NotFoundException(string message) : Exception(message);
public class DatabaseCrudFailedException(string message) : Exception(message);