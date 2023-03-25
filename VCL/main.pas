unit main;

interface

uses
  Forms, StdCtrls, Controls, ComCtrls, Classes, wclBluetooth;

type
  TfmMain = class(TForm)
    btEnumDevices: TButton;
    lvDevices: TListView;
    laNewName: TLabel;
    edNewName: TEdit;
    btSetName: TButton;
    BluetoothManager: TwclBluetoothManager;
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btEnumDevicesClick(Sender: TObject);
    procedure btSetNameClick(Sender: TObject);

  private
    FRadio: TwclBluetoothRadio;
  end;

var
  fmMain: TfmMain;

implementation

uses
  Windows, wclErrors, Dialogs, SysUtils;

{$R *.dfm}

const
  BLUETOOTH_MAX_NAME_SIZE = 248;

type
  BTH_ADDR = Int64;

  BLUETOOTH_ADDRESS = record
    case Byte of
      0: ( ullLong: BTH_ADDR; );
      1: ( rgBytes: array [0..5] of BYTE; );
  end;

  PBLUETOOTH_DEVICE_INFO = ^BLUETOOTH_DEVICE_INFO;
  BLUETOOTH_DEVICE_INFO = record
    dwSize: DWORD;
    Address: BLUETOOTH_ADDRESS;
    ulClassofDevice: ULONG;
    fConnected: BOOL;
    fRemembered: BOOL;
    fAuthenticated: BOOL;
    stLastSeen: SYSTEMTIME;
    stLastUsed: SYSTEMTIME;
    szName: array [0..BLUETOOTH_MAX_NAME_SIZE - 1] of WideChar;
  end;

function BluetoothUpdateDeviceRecord(
  pbtdi: PBLUETOOTH_DEVICE_INFO): DWORD; stdcall; external 'BluetoothApis.dll';

procedure TfmMain.FormCreate(Sender: TObject);
begin
  BluetoothManager.Open;
  if BluetoothManager.GetClassicRadio(FRadio) <> WCL_E_SUCCESS then
    FRadio := nil;
end;

procedure TfmMain.FormDestroy(Sender: TObject);
begin
  BluetoothManager.Close;
end;

procedure TfmMain.btEnumDevicesClick(Sender: TObject);
var
  Devices: TwclBluetoothAddresses;
  i: Integer;
  Item: TListItem;
  Name: string;
begin
  lvDevices.Items.Clear;

  if FRadio = nil then
    ShowMessage('Bluetooth Hardware Not Found.')

  else begin
    if FRadio.EnumPairedDevices(Devices) <> WCL_E_SUCCESS then
      ShowMessage('Enumerating installed devices failed.')

    else begin
      if Length(Devices) = 0 then
        ShowMessage('No Paired devices found.')

      else begin
        for i := 0 to Length(Devices) - 1 do begin
          Item := lvDevices.Items.Add;
          Item.Caption := IntToHex(Devices[i], 12);

          if FRadio.GetRemoteName(Devices[i], Name) <> WCL_E_SUCCESS then
            Name := '<' + IntToHex(Devices[i], 12) + '>';
          Item.SubItems.Add(Name);
        end;
      end;
    end;
  end;
end;

procedure TfmMain.btSetNameClick(Sender: TObject);
var
  Mac: Int64;
  Info: BLUETOOTH_DEVICE_INFO;
  Name: string;
  UniName: WideString;
  Res: Integer;
begin
  if FRadio = nil then
    ShowMessage('Bluetooth Hardware Not Found.')

  else begin
    if lvDevices.Selected = nil then
      ShowMessage('Select device')

    else begin
      if edNewName.Text = '' then
        ShowMessage('Name can not be empty')

      else begin
        Mac := StrToInt64('$' + lvDevices.Selected.Caption);
        ZeroMemory(@Info, SizeOf(BLUETOOTH_DEVICE_INFO));
        Info.dwSize := SizeOf(BLUETOOTH_DEVICE_INFO);
        Info.Address.ullLong := Mac;

        Name := edNewName.Text;
        if Length(Name) > BLUETOOTH_MAX_NAME_SIZE - 2 then
          SetLength(Name, BLUETOOTH_MAX_NAME_SIZE - 2);
        UniName := WideString(Name);
        CopyMemory(@Info.szName[0], Pointer(UniName),
          Length(UniName) * SizeOf(WideChar));

        Res := BluetoothUpdateDeviceRecord(@Info);
        if Res <> 0 then
          ShowMessage('Update device name failed')

        else begin
          if FRadio.GetRemoteName(Mac, Name) <> WCL_E_SUCCESS then
            Name := '<' + IntToHex(Mac, 12) + '>';
          lvDevices.Selected.SubItems[0] := Name;

          // We need to re-enable Bluetooth so name will be refreshed.
          if FRadio.TurnOff = WCL_E_SUCCESS then begin
            if FRadio.TurnOn <> WCL_E_SUCCESS then
              ShowMessage('Enable Bluetooth.');
          end;

          ShowMessage('Name changed!');
        end;
      end;
    end;
  end;
end;

end.
