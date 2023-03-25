object fmMain: TfmMain
  Left = 544
  Top = 155
  BorderStyle = bsSingle
  Caption = 'Bluetooth Device Renamer'
  ClientHeight = 235
  ClientWidth = 345
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  PixelsPerInch = 96
  TextHeight = 13
  object laNewName: TLabel
    Left = 8
    Top = 208
    Width = 54
    Height = 13
    Caption = 'New name:'
  end
  object btEnumDevices: TButton
    Left = 8
    Top = 8
    Width = 97
    Height = 25
    Caption = 'Enum devices'
    TabOrder = 0
    OnClick = btEnumDevicesClick
  end
  object lvDevices: TListView
    Left = 8
    Top = 40
    Width = 329
    Height = 150
    Columns = <
      item
        Caption = 'Device address'
        Width = 150
      end
      item
        Caption = 'Device name'
        Width = 150
      end>
    GridLines = True
    ReadOnly = True
    RowSelect = True
    TabOrder = 1
    ViewStyle = vsReport
  end
  object edNewName: TEdit
    Left = 72
    Top = 200
    Width = 185
    Height = 21
    TabOrder = 2
  end
  object btSetName: TButton
    Left = 264
    Top = 200
    Width = 75
    Height = 25
    Caption = 'Set name'
    TabOrder = 3
    OnClick = btSetNameClick
  end
  object BluetoothManager: TwclBluetoothManager
    Left = 88
    Top = 96
  end
end
