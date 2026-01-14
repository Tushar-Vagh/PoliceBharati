import React, { forwardRef, useEffect, useState } from "react";

// ⚠️ STYLES UNCHANGED
const styles = {
    form: {
        fontFamily: "Arial, sans-serif",
        padding: "40px",
        lineHeight: "1.6",
        maxWidth: '800px',
        margin: '0 auto',
        color: '#000'
    },
    center: { textAlign: "center" },
    bold: { fontWeight: "bold" },
    input: {
        border: 'none',
        borderBottom: '1px solid #000',
        outline: 'none',
        padding: '0 5px',
        width: '200px',
        background: 'transparent'
    },
    checkbox: { marginRight: '10px' }
};

const RejectionSlip = forwardRef(({ candidate, reasons = [] }, ref) => {
    const [master, setMaster] = useState(null);
    const currentDate = new Date().toLocaleDateString('en-IN');

    /* ================= FETCH MASTER DATA ================= */
    useEffect(() => {
        if (!candidate?.application_number) return;

        const fetchMaster = async () => {
            try {
                const res = await fetch(
                    `/api/Master/by-application/${candidate.application_number}`
                );
                if (!res.ok) return;
                const data = await res.json();
                setMaster(data);
            } catch (err) {
                console.error("Failed to fetch master data", err);
            }
        };

        fetchMaster();
    }, [candidate]);

    if (!candidate || !master) return null;

    /* ================= FULL NAME HELPERS ================= */
    const fullNameEnglish =
        `${master.firstName_English || ''} ${master.fatherName_English || ''} ${master.surname_English || ''}`.trim();

    const fullNameMarathi =
        `${master.firstName_Marathi || ''} ${master.fatherName_Marathi || ''} ${master.surname_Marathi || ''}`.trim();

    return (
        <div ref={ref} style={styles.form}>

            <h2 style={styles.center}>महाराष्ट्र / पोलीस</h2>
            <p style={styles.center}>सद्रक्षणाय खलनिगृहणाय</p>

            <h3 style={styles.center}>
                पोलीस अधीक्षक कार्यालय, अहिल्यानगर (आस्थापना शाखा)
            </h3>

            <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.9rem' }}>
                <div>
                    <p>दुरध्वनी क्र. ०२४१ २४१६११३</p>
                    <p>फॅक्स क्र. ०२४१-२४१६१३३</p>
                </div>
                <div style={{ textAlign: 'right' }}>
                    <p>वेब: www.ahmednagarpolice.org</p>
                    <p>Email: sp.ahmednagar@mahapolice.gov.in</p>
                </div>
            </div>

            <hr style={{ border: '1px solid #000' }} />

            <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                <p>क्रमांक : आशा/१०७/४.३/पो.भ.२०२४-२५/</p>
                <p>दिनांक : <span>{currentDate}</span></p>
            </div>

            <p>/२०२६ अहिल्यानगर</p>

            <br />
            <p>प्रति,</p>
            <p>श्री / श्रीमती <b>{fullNameMarathi}</b></p>

            <label>
                आवेदन अर्ज क्रमांक : <b>{master.applicationNo}</b>
            </label>

            <h4 style={{ textDecoration: 'underline' }}>
                विषय :- पोलीस भरती-२०२४-२५ प्रक्रियेत शारीरिक पात्रतेत अपात्र ठरल्याबाबत.
            </h4>

            <p>
                महोदय / महोदया,<br />
                उपरोक्त विषयान्वये आपणास कळविण्यात येते की, पोलीस भरती-२०२४-२५ मध्ये आपण खालील
                नमुद प्रमाणे खुण ( ✓ ) केलेल्या कारणास्तव शारीरिक पात्रतेत अपात्र ठरलेले आहात.
            </p>

            <h4>अपात्र ठरण्याचे कारण</h4>
            <ol style={{ listStyle: 'decimal', paddingLeft: '20px' }}>
                <li>
                    <span>{reasons.includes('height') ? '✓' : ' '}</span>
                    &nbsp;उंची विहित मर्यादेपेक्षा कमी
                </li>
                <li>
                    <span>{reasons.includes('chest') ? '✓' : ' '}</span>
                    &nbsp;छाती न फुगविता ७९ से.मी. पेक्षा कमी / छाती ०५ से.मी. पेक्षा कमी फुगते
                </li>
                <li>शैक्षणिक अर्हता पूर्ण नाही (Document Verification Failure)</li>
            </ol>

            <br /><br />
            <h4 style={{ textAlign: 'center', textDecoration: 'underline' }}>बंधपत्र</h4>

            <p>
                मी <b>{fullNameMarathi}</b>, आवेदन अर्ज क्रमांक <b>{master.applicationNo}</b>,
                अहिल्यानगर (पोलीस भरती - २०२४-२५) घटकात पोलीस भरतीच्या मैदानी चाचणीसाठी
                उपस्थित राहिलो होतो. मला वरील नमुद कारणासाठी भरती प्रकियेत अपात्र केलेले आहे.
                मला अपात्र केलेले मान्य असुन त्याबाबत मला स्पष्ट समज मिळाली आहे.
            </p>

            <div style={{ marginTop: '40px', display: 'flex', justifyContent: 'space-between' }}>
                <div>
                    <p>उमेदवाराचे नांव : _______________________</p>
                    <p>मोबाईल क्रमांक : {master.mobileNo || '________________'}</p>
                </div>
                <div>
                    <p>सही : _______________________</p>
                    <p>दिनांक : {currentDate}</p>
                </div>
            </div>

            <div style={{ marginTop: '40px', textAlign: 'center' }}>
                <p>प्रभारी अधिकारी</p>
                <p>शारीरिक मोजमाप कक्ष</p>
            </div>

        </div>
    );
});

export default RejectionSlip;
