import React from "react";
import '../styles/RejectionForm.css';

const PoliceRecruitmentForm = ({ data }) => {
  const today = new Date().toLocaleDateString('mr-IN');

  return (
    <div className="rejection-paper-container">
      <div className="police-form-exact">
        
        {/* HEADER SECTION */}
        <div className="header-section">
          <h2>महाराष्ट्र / पोलीस</h2>
          <p className="motto">सद्रक्षणाय खलनिगृहणाय</p>
          <h3>पोलीस अधीक्षक कार्यालय, अहिल्यानगर (आस्थापना शाखा)</h3>
        </div>

        <div className="contact-info-strip">
          <div className="contact-left">
            <p>दुरध्वनी क्र. ०२४१ २४१६११३</p>
            <p>फॅक्स क्र. ०२४१-२४१६१३३</p>
          </div>
          <div className="contact-right">
            <p>वेब: www.ahmednagarpolice.org</p>
            <p>Email: sp.ahmednagar@mahapolice.gov.in</p>
          </div>
        </div>

        {/* RESTORED EXACT TEXT FROM YOUR ORIGINAL */}
        <div className="reference-section">
          <p>क्रमांक : आशा/१०७/४.३/पो.भ.२०२४-२५/</p>
          <p>/२०२६ अहिल्यानगर</p>
          <p>जवळ शुद्ध त्रयोदशी</p>
          <p>स्वस्ति श्रीराज्याभिषेक शाके ३५०</p>
        </div>

        <div className="input-row">
          <label>दिनांक :</label>
          <input type="text" value={today} readOnly className="dotted-input" />
        </div>

        <p className="margin-v">प्रति, श्री / श्रीमती ................................................................</p>

        <div className="input-row">
          <label>आवेदन अर्ज क्रमांक :</label>
          <input type="text" value={data?.application_number || ""} readOnly className="dotted-input" />
        </div>

        <h4 className="subject-line">
          विषय :- पोलीस भरती-२०२४-२५ प्रक्रियेत शारीरिक पात्रतेत अपात्र ठरल्याबाबत.
        </h4>

        <p className="intro-text">
          महोदय / महोदया,<br />
          उपरोक्त विषयान्वये आपणास कळविण्यात येते की, पोलीस भरती-२०२४-२५ मध्ये आपण खालील
          नमुद प्रमाणे खुण ( ✓ ) केलेल्या कारणास्तव शारीरिक पात्रतेत अपात्र ठरलेले आहात.
        </p>

        <h4 className="sub-title">अपात्र ठरण्याचे कारण</h4>
        <ol className="rejection-list">
          <li><span className="check-box">{data?.height_cm < 165 ? '✓' : ''}</span> १. उंची विहित मर्यादेपेक्षा कमी</li>
          <li><span className="check-box">{data?.chest_cm < 79 ? '✓' : ''}</span> २. छाती न फुगविता ७९ से.मी. पेक्षा कमी</li>
          <li><span className="check-box"></span> ३. छाती ०५ से.मी. पेक्षा कमी फुगते</li>
          <li><span className="check-box"></span> ४. शैक्षणिक अर्हता पूर्ण नाही</li>
          <li><span className="check-box"></span> ५. वयोमर्यादा पूर्ण नाही</li>
          <li><span className="check-box"></span> ६. जातीचे प्रमाणपत्र नाही</li>
          <li><span className="check-box"></span> ७. नॉन-क्रिमीलेयर प्रमाणपत्र नाही</li>
          <li><span className="check-box"></span> ८. माजी सैनिक प्रमाणपत्र नाही</li>
        </ol>

        {/* BOND SECTION */}
        <h4 className="center-text">बंधपत्र</h4>

        <div className="input-row">
          <label>दिनांक :</label>
          <input type="text" value={today} readOnly className="dotted-input" />
        </div>

        <div className="input-row">
          <label>उमेदवाराचे नांव :</label>
          <input type="text" value={data?.candidate_name || ""} readOnly className="dotted-input" style={{width: '300px'}} />
        </div>

        <div className="input-row">
          <label>आवेदन अर्ज क्रमांक :</label>
          <input type="text" value={data?.application_number || ""} readOnly className="dotted-input" />
        </div>

        <p className="bond-body">
          मी नाशिक ग्रामीण (पोलीस भरती - २०२४-२५) घटकात पोलीस भरतीच्या मैदानी चाचणीसाठी
          उपस्थित राहिलो होतो. मला खालील नमुद कारणासाठी भरती प्रकियेत अपात्र केलेले आहे. मला
          अपात्र केलेले मान्य असुन त्याबाबत मला स्पष्ट समज मिळाली आहे.
        </p>

        <h4 className="sub-title">अपात्र ठरण्याचे कारण</h4>

        <div className="input-row">
          <label>दिनांक :</label>
          <input type="text" value={today} readOnly className="dotted-input" />
        </div>

        <div className="input-row">
          <label>सही :</label>
          <input type="text" placeholder="...................................." readOnly className="dotted-input" />
        </div>

        <div className="input-row">
          <label>मोबाईल क्रमांक :</label>
          <input type="text" value={data?.mobile_number || ""} readOnly className="dotted-input" />
        </div>

        <div className="no-print" style={{ textAlign: 'center', marginTop: '15px' }}>
          <button className="btn-print-small" onClick={() => window.print()}>प्रिंट करा</button>
        </div>
      </div>
    </div>
  );
};

export default PoliceRecruitmentForm;