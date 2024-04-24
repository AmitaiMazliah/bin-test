using ClassLibrary;

namespace TestProject1;

public class Tests
{
    [Test]
    public void Test1()
    {
        const string filePath = "secret.txt";
        
        var permissions = FileUtils.GetFilePermissions(filePath);
        Assert.AreEqual("-rw-r-----", permissions);
        
        var firstRegisterRequestId = Engine.Register(filePath, true, false);
        permissions = FileUtils.GetFilePermissions(filePath);
        Assert.AreEqual("-rw-r--r--", permissions);
        Assert.AreEqual(1, firstRegisterRequestId);
        
        var secondRegisterRequestId = Engine.Register(filePath, false, true);
        permissions = FileUtils.GetFilePermissions(filePath);
        Assert.AreEqual("-rw-r--rw-", permissions);
        Assert.AreEqual(2, secondRegisterRequestId);
        
        Engine.Unregister(firstRegisterRequestId);
        permissions = FileUtils.GetFilePermissions(filePath);
        Assert.AreEqual("-rw-r---w-", permissions);
        
        Engine.Unregister(secondRegisterRequestId);
        permissions = FileUtils.GetFilePermissions(filePath);
        Assert.AreEqual("-rw-r-----", permissions);
        
        var thirdRegisterRequestId = Engine.Register(filePath, true, true);
        permissions = FileUtils.GetFilePermissions(filePath);
        Assert.AreEqual("-rw-r--rw-", permissions);
        Assert.AreEqual(3, thirdRegisterRequestId);
        
        var forthRegisterRequestId = Engine.Register(filePath, true, false);
        permissions = FileUtils.GetFilePermissions(filePath);
        Assert.AreEqual("-rw-r--rw-", permissions);
        Assert.AreEqual(4, forthRegisterRequestId);
        
        Engine.Unregister(thirdRegisterRequestId);
        permissions = FileUtils.GetFilePermissions(filePath);
        Assert.AreEqual("-rw-r--r--", permissions);
        
        Engine.Unregister(thirdRegisterRequestId);
        permissions = FileUtils.GetFilePermissions(filePath);
        Assert.AreEqual("-rw-r--r--", permissions);
        
        Engine.Unregister(forthRegisterRequestId);
        permissions = FileUtils.GetFilePermissions(filePath);
        Assert.AreEqual("-rw-r-----", permissions);
        
        var fifthRegisterRequestId = Engine.Register("xxxxxx", true, true);
        permissions = FileUtils.GetFilePermissions(filePath);
        Assert.AreEqual("-rw-r-----", permissions);
        Assert.AreEqual(5, fifthRegisterRequestId);
    }
}