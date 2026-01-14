import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/pst.css";

const MASTER_API = "http://localhost:5000/api/Master";
const ITEMS_PER_PAGE = 5;

export default function PstCandidateList() {
  const navigate = useNavigate();
  

  const [applicants, setApplicants] = useState([]);
  const [loading, setLoading] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");
  const [currentPage, setCurrentPage] = useState(1);

  useEffect(() => {
    const load = async () => {
      setLoading(true);
      const res = await fetch(MASTER_API);
      const data = await res.json();
      setApplicants(data);
      setLoading(false);
    };
    load();
  }, []);

  const filtered = applicants.filter(a => {
    const s = searchTerm.toLowerCase();
    return (
      a.applicationNo?.toLowerCase().includes(s) ||
      `${a.firstName_English} ${a.surname_English}`.toLowerCase().includes(s) ||
      `${a.firstName_Marathi} ${a.surname_Marathi}`.toLowerCase().includes(s)
    );
  });

  const totalPages = Math.ceil(filtered.length / ITEMS_PER_PAGE);
  const paginated = filtered.slice(
    (currentPage - 1) * ITEMS_PER_PAGE,
    currentPage * ITEMS_PER_PAGE
  );

  return (
    <div className="pst-full-page">
      <div className="applicant-table-wrapper">
        <div className="table-search">
          <input
            placeholder="Search by Application No or Name"
            value={searchTerm}
            onChange={(e) => {
              setSearchTerm(e.target.value);
              setCurrentPage(1);
            }}
          />
        </div>

        {loading ? (
          <p>Loading...</p>
        ) : paginated.length === 0 ? (
          <div className="no-record-box">❌ Applicant not found</div>
        ) : (
          <>
            <table className="applicant-table">
              <thead>
                <tr>
                  <th>Application No</th>
                  <th>Name (English)</th>
                  <th>Name (Marathi)</th>
                  <th>Action</th>
                </tr>
              </thead>
              <tbody>
                {paginated.map(a => (
                  <tr key={a.applicationNo}>
                    <td>{a.applicationNo}</td>
                    <td>{a.firstName_English} {a.surname_English}</td>
                    <td>{a.firstName_Marathi} {a.surname_Marathi}</td>
                    <td>
                      <button
                        className="btn-primary"
                        onClick={() =>
                          navigate(`/pst/${a.applicationNo}`)
                        }
                      >
                        View
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>

            {totalPages > 1 && (
              <div className="pagination-controls">
                <button
                  disabled={currentPage === 1}
                  onClick={() => setCurrentPage(p => p - 1)}
                >
                  Previous
                </button>
                <span>Page {currentPage} of {totalPages}</span>
                <button
                  disabled={currentPage === totalPages}
                  onClick={() => setCurrentPage(p => p + 1)}
                >
                  Next
                </button>
              </div>
            )}
          </>
        )}
      </div>
    </div>
  );
}
