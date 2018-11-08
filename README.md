# .NET wrapper for Sberbank pilot_nt.dll

## Usage:

### Payment

<pre><code>

var pilot = new PilotNt();            
decimal amount = .01m;

var paymentRes = pilot.Payment(amount);
if (!paymentRes.IsSuccess)
{
    Console.WriteLine("Payment was not successful. Error code: {0}, {1}",
        paymentRes.ResponseCode, paymentRes.ErrorMessage);
    return;
}

if (paymentRes.Checks != null)
{
    foreach (var check in paymentRes.Checks)
    {
        Console.WriteLine("Printing check in not-fiscal mode");
        Console.WriteLine(check);
    }
}

if (!pilot.CommitTransaction(amount, paymentRes.AuthCode))
{
    Console.WriteLine("Commit transaction error");
    return;
}

</code></pre>

### Return\cancel payment

<pre><code>

var returnRes = pilot.Return(amount);
if (!returnRes.IsSuccess)
{
    Console.WriteLine("Payment return error");
    return;
}

if (returnRes.Checks != null)
{
    foreach (var check in returnRes.Checks)
    {
        Console.WriteLine("Printing check in not-fiscal mode");
        Console.WriteLine(check);
    }
}

</code></pre>

### Close shift (print report)

<pre><code>

var closeShiftRes = pilot.CloseShift();
if (closeShiftRes.Checks != null)
{
    foreach (var check in closeShiftRes.Checks)
    {
        Console.WriteLine("Printing check in not-fiscal mode");
        Console.WriteLine(check);
    }
}

</code></pre>

