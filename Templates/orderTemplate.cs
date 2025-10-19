using System;
using TAS_Test.Models;

namespace TAS_Test;

class orderTemplate
{
       public static string html(Order order) => $@"
            <!DOCTYPE html>
            <html lang=""de"">
            <head>
                <meta charset=""UTF-8"">
                <title>Auftrag - {order.auftragsnamen}</title>
                <style>
                    body {{
                        font-family: 'Helvetica Neue', Arial, sans-serif;
                        margin: 40px;
                        color: #222;
                    }}

                    h1 {{
                        text-align: center;
                        font-size: 28px;
                        color: #1a1a1a;
                        margin-bottom: 10px;
                        border-bottom: 3px solid #333;
                        padding-bottom: 5px;
                   }}

                    .info-block {{
                        background: #f6f6f6;
                        border: 1px solid #ddd;
                        padding: 15px 20px;
                        border-radius: 8px;
                        margin-bottom: 20px;
                    }}

                    .info-line {{
                        display: flex;
                        justify-content: space-between;
                        margin: 5px 0;
                    }}

                    .info-line span {{
                        font-weight: 500;
                    }}

                    .section {{
                        margin-top: 30px;
                    }}

                    .section h2 {{
                        font-size: 20px;
                        margin-bottom: 8px;
                        border-left: 4px solid #555;
                        padding-left: 8px;
                        color: #333;
                    }}

                    .todo-box {{
                        border: 1px solid #bbb;
                        background: #fafafa;
                        padding: 15px;
                        border-radius: 6px;
                        min-height: 100px;
                        white-space: pre-wrap;
                    }}

                    .footer {{
                        margin-top: 50px;
                        border-top: 1px solid #555;
                        padding-top: 20px;
                        text-align: center;
                        font-size: 12px;
                        color: #555;
                    }}

                    .signature {{
                        margin-top: 60px;
                        display: flex;
                        justify-content: space-between;
                        font-size: 13px;
                    }}

                    .signature .line {{
                        width: 40%;
                        border-top: 1px solid #000;
                        text-align: center;
                        padding-top: 5px;
                    }}
                </style>
            </head>
            <body>
                <h1>{order.auftragsnamen}</h1>

                <div class=""info-block"">
                    <div class=""info-line""><span>Auftragsnummer:</span> {order.order_id}</div>
                    <div class=""info-line""><span>Kundennummer:</span> {order.k_id}</div>
                    <div class=""info-line""><span>Muss fertig sein bis:</span> {order.auftragsdatum}</div>                  
                </div>

                <div class=""info-block"">
                    <div class=""info-line""><span>Kunde:</span> {order.name}</div>
                    <div class=""info-line""><span>Fahrzeug:</span> {order.name}</div>
                    <div class=""info-line""><span>E-Mail:</span> {order.mail}</div>
                    <div class=""info-line""><span>Telefon:</span> {order.phone}</div>
                </div>

                <div class=""section"">
                    <h2>To-Do</h2>
                    <div class=""todo-box"">{order.orderNotes}</div>
                </div>

                <div class=""section"">
                    <h2>Kosten</h2>
                    <p>Maximale Kosten laut Auftrag: <strong>{order.maxKosten} €</strong></p>
                </div>

                <div class=""signature"">
                    <div class=""line"">Ort, Datum</div>
                    <div class=""line"">Unterschrift Kunde</div>
                </div>

                <div class=""footer"">
                    <p>Tilmanns Car Detailling</p>
                </div>
            </body>
            </html>
                        ";
}

