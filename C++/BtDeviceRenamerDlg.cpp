// BtDeviceRenamerDlg.cpp : implementation file
//

#include "stdafx.h"
#include "BtDeviceRenamer.h"
#include "BtDeviceRenamerDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

#ifndef BLUETOOTH_MAX_NAME_SIZE
	#define BLUETOOTH_MAX_NAME_SIZE	248
#endif

#ifndef BTH_ADDR
	typedef LONGLONG BTH_ADDR;
#endif;

#ifndef BLUETOOTH_ADDRESS
	typedef struct
	{
		union
		{
			BTH_ADDR	ullLong;
			BYTE		rgBytes[6];
		};
	} BLUETOOTH_ADDRESS;
#endif

#ifndef BLUETOOTH_DEVICE_INFO
	typedef struct
	{
		DWORD				dwSize;
		BLUETOOTH_ADDRESS	Address;
		ULONG				ulClassofDevice;
		BOOL				fConnected;
		BOOL				fRemembered;
		BOOL				fAuthenticated;
		SYSTEMTIME			stLastSeen;
		SYSTEMTIME			stLastUsed;
		WCHAR				szName[BLUETOOTH_MAX_NAME_SIZE];
	} BLUETOOTH_DEVICE_INFO, *PBLUETOOTH_DEVICE_INFO;
#endif

typedef DWORD (__stdcall *PBluetoothUpdateDeviceRecord)(PBLUETOOTH_DEVICE_INFO pbtdi);

PBluetoothUpdateDeviceRecord _BluetoothUpdateDeviceRecord = NULL;

DWORD BluetoothUpdateDeviceRecord(PBLUETOOTH_DEVICE_INFO pbtdi)
{
	if (_BluetoothUpdateDeviceRecord != NULL)
		return _BluetoothUpdateDeviceRecord(pbtdi);
	return ERROR_INVALID_FUNCTION;
}

// CBtDeviceRenamerDlg dialog




CBtDeviceRenamerDlg::CBtDeviceRenamerDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CBtDeviceRenamerDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CBtDeviceRenamerDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_DEVICES, lvDevices);
	DDX_Control(pDX, IDC_EDIT_NEW_NAME, edNewName);
}

BEGIN_MESSAGE_MAP(CBtDeviceRenamerDlg, CDialog)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_WM_DESTROY()
	ON_BN_CLICKED(IDC_BUTTON_ENUM_DEVICES, &CBtDeviceRenamerDlg::OnBnClickedButtonEnumDevices)
	ON_BN_CLICKED(IDC_BUTTON_SET_NAME, &CBtDeviceRenamerDlg::OnBnClickedButtonSetName)
END_MESSAGE_MAP()


// CBtDeviceRenamerDlg message handlers

BOOL CBtDeviceRenamerDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here
	FLib = LoadLibrary(_T("BluetoothApis.dll"));
	if (FLib != NULL)
	{
		_BluetoothUpdateDeviceRecord = (PBluetoothUpdateDeviceRecord)GetProcAddress(FLib,
		"BluetoothUpdateDeviceRecord");
	}

	FManager = new CwclBluetoothManager();
	FManager->Open();
	if (FManager->GetClassicRadio(FRadio) != WCL_E_SUCCESS)
		FRadio = NULL;

	lvDevices.InsertColumn(0, _T("Device address"), 0, 150);
	lvDevices.InsertColumn(1, _T("Device name"), 0, 150);

	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CBtDeviceRenamerDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CBtDeviceRenamerDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}


void CBtDeviceRenamerDlg::OnDestroy()
{
	CDialog::OnDestroy();

	FManager->Close();

	// TODO: Add your message handler code here
	if (FLib != NULL)
		FreeLibrary(FLib);
}

void CBtDeviceRenamerDlg::OnBnClickedButtonEnumDevices()
{
	// TODO: Add your control notification handler code here
	lvDevices.DeleteAllItems();
	
	if (FRadio == NULL)
		AfxMessageBox(_T("Bluetooth Hardware Not Found."));
	else
	{
		wclBluetoothAddresses Devices;
		if (FRadio->EnumPairedDevices(Devices) != WCL_E_SUCCESS)
			AfxMessageBox(_T("Enumerating installed devices failed."));
		else
		{
			if (Devices.size() == 0)
				AfxMessageBox(_T("No Paired devices found."));
			else
			{
				for (wclBluetoothAddresses::iterator Mac = Devices.begin(); Mac != Devices.end(); Mac++)
				{
					CString s;
					s.Format(_T("%.4X%.8X"), static_cast<INT32>(((*Mac) >> 32) & 0x00000FFFF),
						static_cast<INT32>((*Mac)) & 0xFFFFFFFF);
					
					int Item = lvDevices.GetItemCount();
					lvDevices.InsertItem(Item, s);
					
					tstring Name;
					if (FRadio->GetRemoteName((*Mac), Name) != WCL_E_SUCCESS)
						Name = _T("<") + tstring(s.GetBuffer()) + _T(">");
					lvDevices.SetItemText(Item, 1, Name.c_str());

					Item++;
				}
			}
		}
	}
}

void CBtDeviceRenamerDlg::OnBnClickedButtonSetName()
{
	// TODO: Add your control notification handler code here
	if (FRadio == NULL)
		AfxMessageBox(_T("Bluetooth Hardware Not Found."));
	else
	{
		POSITION Pos = lvDevices.GetFirstSelectedItemPosition();
		if (Pos == NULL)
			AfxMessageBox(_T("Select device"));
		else
		{
			int Item = lvDevices.GetNextSelectedItem(Pos);

			CString s;
			edNewName.GetWindowText(s);
			if (s == _T(""))
				AfxMessageBox(_T("Name can not be empty"));
			else
			{
				CString Addr = lvDevices.GetItemText(Item, 0);
				__int64 Mac = _tcstoi64(Addr, NULL, 16);
				BLUETOOTH_DEVICE_INFO Info;
				ZeroMemory(&Info, sizeof(BLUETOOTH_DEVICE_INFO));
				Info.dwSize = sizeof(BLUETOOTH_DEVICE_INFO);
				Info.Address.ullLong = Mac;

				if (s.GetLength() > BLUETOOTH_MAX_NAME_SIZE - 2)
					AfxMessageBox(_T("Name too long"));
				else
				{
					CopyMemory(Info.szName, s.GetBuffer(), s.GetLength() * sizeof(WCHAR));
					
					DWORD Res = BluetoothUpdateDeviceRecord(&Info);
					if (Res != 0)
						AfxMessageBox(_T("Update device name failed"));
					else
					{
						tstring Name;
						if (FRadio->GetRemoteName(Mac, Name) != WCL_E_SUCCESS)
						{
							s.Format(_T("%.4X%.8X"), static_cast<INT32>((Mac >> 32) & 0x00000FFFF),
								static_cast<INT32>(Mac) & 0xFFFFFFFF);
							Name = _T("<") + tstring(s.GetBuffer()) + _T(">");
						}
						
						lvDevices.SetItemText(Item, 1, Name.c_str());
						
						// We need to re-enable Bluetooth so name will be refreshed.
						if (FRadio->TurnOff() == WCL_E_SUCCESS)
						{
							if (FRadio->TurnOn() != WCL_E_SUCCESS)
								AfxMessageBox(_T("Enable Bluetooth."));
						}
						AfxMessageBox(_T("Name changed!"));
					}
				}
			}
		}
	}
}
