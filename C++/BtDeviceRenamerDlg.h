// BtDeviceRenamerDlg.h : header file
//

#pragma once
#include "afxcmn.h"
#include "afxwin.h"

#include ".\\Lib\\Bluetooth\\wclBluetooth.h"

using namespace wclCommon;
using namespace wclBluetooth;


// CBtDeviceRenamerDlg dialog
class CBtDeviceRenamerDlg : public CDialog
{
// Construction
public:
	CBtDeviceRenamerDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_BTDEVICERENAMER_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()

private:
	CListCtrl lvDevices;
	CEdit edNewName;

private:
	CwclBluetoothManager* FManager;
	CwclBluetoothRadio* FRadio;
	HMODULE FLib;
public:
	afx_msg void OnDestroy();
	afx_msg void OnBnClickedButtonEnumDevices();
	afx_msg void OnBnClickedButtonSetName();
};
