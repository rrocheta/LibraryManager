import React from "react";

const toneClassMap = {
  error: "error",
  success: "success",
  info: "",
};

export default function StatusMessage({ tone = "info", children }) {
  if (!children) return null;

  const role = tone === "error" ? "alert" : "status";
  const ariaLive = tone === "error" ? "assertive" : "polite";
  const toneClass = toneClassMap[tone] || "";

  return (
    <div className={`state ${toneClass}`.trim()} role={role} aria-live={ariaLive}>
      {children}
    </div>
  );
}
