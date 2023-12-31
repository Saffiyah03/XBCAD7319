using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace api.email;
public interface IMailService
{
    Task SendEmailUser(MailRequest mailRequest);

    Task SendEmailAdmin(MailRequest mailRequest);

    Task SendEmailAsync(MailRequest mailRequest);

    Task<string> StoreAttachments(List<IFormFile> files);

}