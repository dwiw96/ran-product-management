namespace ran_product_management_net.Utils;

public class NotFoundException(string message) : Exception(message);
public class DatabaseCrudFailedException(string message) : Exception(message);
public class NotNullException(string message) : Exception(message);
public class DuplicateDataException(string message) : Exception(message);
public class LengthException(string message) : Exception(message);
public class ServerErrorException(string message) : Exception(message);
public class NotImplementedException(string message) : Exception(message);
public class InvalidParameterException(string message) : Exception(message);