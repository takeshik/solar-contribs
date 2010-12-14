import clr
clr.AddReference("System.Core")

from System import *
from System.Collections.Generic import *
from System.Linq import *
from System.Windows import *
from System.Windows.Controls import *
from System.Windows.Input import *
from Solar import *
from Solar.Models import *
from Lunar import *
from System.Linq import *

def selectAllInput(sender, e):
        from System.Windows.Input import *
        
        if e.Key == Key.A and e.KeyboardDevice.Modifiers == ModifierKeys.Control:
            console.TextBox.Select(console.InputStart, console.InputLength)
            e.Handled = True
 
console.PreviewKeyDown += selectAllInput

print "\r\ninit script was loaded."
